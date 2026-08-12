using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using S9BEditor.ViewModels;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interop;

namespace S9BEditor
{
	internal class ShapeViewerProxy
	{
		private enum IPServerState
		{
			Disconnected,
			RequestingReload,
			ReloadSuccess,
			CreatingMatrices,
			Ready,
			RequestingMatrices
		}

		private AutoResetEvent mDocumentChangeLock;

		private static readonly float MATRIX_UPDATE_THRESHOLD = 0.0005f;

		private Thread mDocumentUpdateThread;

		private InterProcessClientAsync mIPC;

		private TypeEditDocument mDocument;

		private bool mDocumentChanging;

		private List<string> mMatrixDatumPaths;

		private List<string> mBoxDatumPaths;

		private List<string> mChildDatumPaths;

		private Dictionary<ITypeDatum, int> mChildDatumToSVIndices;

		private string mChildDatumOwnerPath;

		private List<float[]> mChildMatricesResults;

		private Thread mMainThread;

		private IPServerState mServerState;

		private List<InterProcessMessage> mCreateMatrixMessages;

		private InterProcessMessage mCurMessage;

		private string mReturnedMatrixHex;

		private bool mReturnedMatrixValid;

		private bool mMatrixDataReceived;

		private string mLastIPCResponse;

		public TypeEditDocument Document
		{
			get
			{
				return mDocument;
			}
			set
			{
				mDocumentChanging = true;
				if (mDocumentChangeLock.WaitOne())
				{
					mDocument = value;
					reload();
					mDocumentChangeLock.Set();
				}
				mDocumentChanging = false;
			}
		}

		public bool HasVCArgument { get; set; }

		public bool HasICArgument { get; set; }

		public bool IsShapeViewerOpen => getProcess() != null;

		public ShapeViewerProxy()
		{
			mMainThread = Thread.CurrentThread;
			mDocumentChangeLock = new AutoResetEvent(initialState: true);
			mMatrixDatumPaths = new List<string>();
			mBoxDatumPaths = new List<string>();
			mChildDatumPaths = new List<string>();
			mChildDatumToSVIndices = new Dictionary<ITypeDatum, int>();
			mIPC = new InterProcessClientAsync();
			mIPC.ResponseReceived += mIPC_ResponseReceived;
			mCreateMatrixMessages = new List<InterProcessMessage>();
			mDocumentUpdateThread = new Thread(updateDocument);
			mDocumentUpdateThread.IsBackground = true;
			mDocumentUpdateThread.Start();
		}

		private void updateDocument()
		{
			while (true)
			{
				Thread.Sleep(10);
				if (!mDocumentChangeLock.WaitOne())
				{
					continue;
				}
				if (!mIPC.IsServerPresent())
				{
					mServerState = IPServerState.Disconnected;
				}
				else if (getProcess() == null)
				{
					resetSharedMemState();
				}
				else
				{
					switch (mServerState)
					{
						case IPServerState.Disconnected:
							if (mDocument != null)
							{
								mServerState = IPServerState.RequestingReload;
								reloadShapeMessage();
							}
							break;
						case IPServerState.ReloadSuccess:
						{
							mServerState = IPServerState.CreatingMatrices;
							mCreateMatrixMessages.Clear();
							IClassTypeDatum currentDatum = Document.CurrentDatum;
							mMatrixDatumPaths.Clear();
							mBoxDatumPaths.Clear();
							mChildDatumPaths.Clear();
							mChildDatumToSVIndices.Clear();
							if (currentDatum != null)
							{
								getMatrices(currentDatum, mMatrixDatumPaths);
								getBoxes(currentDatum, mBoxDatumPaths);
								getChildren(currentDatum, mChildDatumPaths, mChildDatumToSVIndices, out mChildDatumOwnerPath);
							}
							int num = 0;
							int num2 = 0;
							while (num2 < mMatrixDatumPaths.Count)
							{
								string text = TypeEditSchemaUtil.ToHex(currentDatum.GetSingleDatum(mMatrixDatumPaths[num2]));
								InterProcessMessage interProcessMessage = new InterProcessMessage(string.Format(CultureInfo.InvariantCulture, "MCreate: {0}={1}", new object[2] { num, text }));
								mCreateMatrixMessages.Add(interProcessMessage);
								mIPC.SendMessage(interProcessMessage);
								interProcessMessage = new InterProcessMessage(string.Format(CultureInfo.InvariantCulture, "MName: {0}={1}", new object[2]
								{
									num,
									mMatrixDatumPaths[num2]
								}));
								mCreateMatrixMessages.Add(interProcessMessage);
								mIPC.SendMessage(interProcessMessage);
								num2++;
								num++;
							}
							int num3 = 0;
							while (num3 < mBoxDatumPaths.Count)
							{
								string text2 = TypeEditSchemaUtil.ToHex(currentDatum.GetSingleDatum(mBoxDatumPaths[num3]));
								InterProcessMessage interProcessMessage2 = new InterProcessMessage(string.Format(CultureInfo.InvariantCulture, "BCreate: {0}={1}", new object[2] { num, text2 }));
								mCreateMatrixMessages.Add(interProcessMessage2);
								mIPC.SendMessage(interProcessMessage2);
								interProcessMessage2 = new InterProcessMessage(string.Format(CultureInfo.InvariantCulture, "BName: {0}={1}", new object[2]
								{
									num,
									mBoxDatumPaths[num3]
								}));
								mCreateMatrixMessages.Add(interProcessMessage2);
								mIPC.SendMessage(interProcessMessage2);
								num3++;
								num++;
							}
							break;
						}
						case IPServerState.CreatingMatrices:
							if (mCreateMatrixMessages.Count == 0)
							{
								mServerState = IPServerState.Ready;
							}
							break;
						case IPServerState.Ready:
						{
							mServerState = IPServerState.RequestingMatrices;
							mChildMatricesResults = new List<float[]>();
							TypeEditDocument document = Document;
							if (document != null)
							{
								for (int i = 0; i < mMatrixDatumPaths.Count; i++)
								{
									try
									{
										VMTypeDatumBase singleDatum = TypeEditSchemaUtil.GetSingleDatum(document.TypeEditContainer.Datum, mMatrixDatumPaths[i]);
										if (singleDatum != null && !updateMatrixVM("Matrix:", i, singleDatum))
										{
											break;
										}
									}
									catch (Exception)
									{
									}
								}
								for (int j = 0; j < mBoxDatumPaths.Count; j++)
								{
									try
									{
										VMTypeDatumBase singleDatum2 = TypeEditSchemaUtil.GetSingleDatum(document.TypeEditContainer.Datum, mBoxDatumPaths[j]);
										if (singleDatum2 != null && !updateMatrixVM("Box:", j + mMatrixDatumPaths.Count, singleDatum2))
										{
											break;
										}
									}
									catch (Exception)
									{
									}
								}
								if (!string.IsNullOrWhiteSpace(mChildDatumOwnerPath))
								{
									try
									{
										VMTypeDatumBase singleDatum3 = TypeEditSchemaUtil.GetSingleDatum(document.TypeEditContainer.Datum, mChildDatumOwnerPath);
										if (singleDatum3 != null)
										{
											_ = singleDatum3.Datum;
											IEnumerable<VMTypeDatumBase> data = TypeEditSchemaUtil.GetData(singleDatum3, ".:*");
											foreach (VMTypeDatumBase item in data)
											{
												ITypeDatum datum = item.Datum;
												if (mChildDatumToSVIndices.TryGetValue(datum, out var value))
												{
													VMTypeDatumBase singleDatum4 = TypeEditSchemaUtil.GetSingleDatum(item, "Matrix");
													if (singleDatum4 != null && !updateMatrixVM("Child:", value, singleDatum4))
													{
														break;
													}
												}
											}
										}
									}
									catch (Exception)
									{
									}
								}
							}
							mServerState = IPServerState.Ready;
							break;
						}
					}
				}
				mDocumentChangeLock.Set();
			}
		}

		private bool updateMatrixVM(string msg, int matIx, VMTypeDatumBase matrixDatumVM)
		{
			string origHex = TypeEditSchemaUtil.ToHex(matrixDatumVM.Datum);
			requestMatrix(msg, matIx, origHex);
			int num = 0;
			while (!mMatrixDataReceived)
			{
				Thread.Sleep(1);
				num++;
				if (num > 1000 || mDocumentChanging)
				{
					break;
				}
			}
			if (mMatrixDataReceived)
			{
				if (mReturnedMatrixValid)
				{
					if (matrixDatumVM.Datum.TypeDescriptor.Name == "cHcR3dBox")
					{
						float[] matrixValues = TypeEditSchemaUtil.HexToFloats(mReturnedMatrixHex);
						if (matrixValues.Length >= 19 && TypeEditSchemaUtil.TryGetBox(matrixDatumVM.Datum as IClassTypeDatum, out var matrix, out var width, out var height, out var depth))
						{
							bool flag = false;
							for (int i = 0; i < 16; i++)
							{
								if (Math.Abs(matrixValues[i] - matrix[i]) > MATRIX_UPDATE_THRESHOLD)
								{
									flag = true;
									break;
								}
							}
							if (Math.Abs(width - matrixValues[16]) > MATRIX_UPDATE_THRESHOLD || Math.Abs(height - matrixValues[17]) > MATRIX_UPDATE_THRESHOLD || Math.Abs(depth - matrixValues[18]) > MATRIX_UPDATE_THRESHOLD)
							{
								flag = true;
							}
							if (flag)
							{
								Dispatcher val = Dispatcher.FromThread(mMainThread);
								val.Invoke((Delegate)(Action)delegate
								{
									TypeEditSchemaUtil.TrySetBox(matrixDatumVM.Datum as IClassTypeDatum, matrixValues, matrixValues[16], matrixValues[17], matrixValues[18]);
									matrixDatumVM.Reload();
								}, new object[0]);
							}
						}
					}
					else
					{
						float[] matrixValues2 = TypeEditSchemaUtil.HexToFloats(mReturnedMatrixHex);
						if (matrixValues2.Length >= 16 && TypeEditSchemaUtil.TryGetMatrix(matrixDatumVM.Datum as IClassTypeDatum, out var matrix2))
						{
							bool flag2 = false;
							for (int num2 = 0; num2 < 16; num2++)
							{
								if (Math.Abs(matrixValues2[num2] - matrix2[num2]) > MATRIX_UPDATE_THRESHOLD)
								{
									flag2 = true;
									break;
								}
							}
							if (flag2)
							{
								Dispatcher val2 = Dispatcher.FromThread(mMainThread);
								val2.Invoke((Delegate)(Action)delegate
								{
									TypeEditSchemaUtil.TrySetMatrix(matrixDatumVM.Datum as IClassTypeDatum, matrixValues2);
									matrixDatumVM.Reload();
								}, new object[0]);
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		private static void getMatrices(IClassTypeDatum ctd, List<string> matrixDatumPaths)
		{
			TypeEditSchemaUtil.VisitDatum(ctd, delegate(TypeEditSchemaUtil.VisitorState state)
			{
				if (state.CurrentDatum is IClassTypeDatum classTypeDatum)
				{
					if (classTypeDatum.TypeDescriptor.Name == "cHcRMatrix4x4")
					{
						matrixDatumPaths.Add(state.CurrentPath);
					}
					else if (classTypeDatum.TypeDescriptor.Name == "cEntityContainerBlueprint")
					{
						state.ContinueMode = TypeEditSchemaUtil.VisitorStateContinueMode.SkipChildren;
					}
				}
			});
		}

		private static void getBoxes(IClassTypeDatum ctd, List<string> boxDatumPaths)
		{
			TypeEditSchemaUtil.VisitDatum(ctd, delegate(TypeEditSchemaUtil.VisitorState state)
			{
				if (state.CurrentDatum is IClassTypeDatum classTypeDatum)
				{
					if (classTypeDatum.TypeDescriptor.Name == "cHcR3dBox")
					{
						boxDatumPaths.Add(state.CurrentPath);
					}
					else if (classTypeDatum.TypeDescriptor.Name == "cEntityContainerBlueprint")
					{
						state.ContinueMode = TypeEditSchemaUtil.VisitorStateContinueMode.SkipChildren;
					}
				}
			});
		}

		private static void getChildren(IClassTypeDatum ctd, List<string> childDatumPaths, Dictionary<ITypeDatum, int> datumToSVIndices, out string ownerPath)
		{
			string outOwner = null;
			TypeEditSchemaUtil.VisitDatum(ctd, delegate(TypeEditSchemaUtil.VisitorState state)
			{
				if (state.CurrentDatum is IClassTypeDatum classTypeDatum && classTypeDatum.TypeDescriptor.Name == "cEntityContainerBlueprint")
				{
					outOwner = state.CurrentPath + "/Children";
					if (classTypeDatum.GetSingleDatum("Children") is IIndexedTypeDatum indexedTypeDatum)
					{
						for (int i = 0; i < indexedTypeDatum.Count; i++)
						{
							ITypeDatum key = indexedTypeDatum[i];
							childDatumPaths.Add(outOwner + ":" + i);
							datumToSVIndices.Add(key, i);
						}
					}
					state.ContinueMode = TypeEditSchemaUtil.VisitorStateContinueMode.Terminate;
				}
			});
			ownerPath = outOwner;
		}

		private void mIPC_ResponseReceived(object sender, InterProcessResponseArgs e)
		{
			mLastIPCResponse = e.Response;
			if (!e.Success)
			{
				mServerState = IPServerState.Disconnected;
			}
			else if (mServerState == IPServerState.CreatingMatrices)
			{
				mCreateMatrixMessages.Remove(e.Message);
			}
			else
			{
				if (e.Message != mCurMessage)
				{
					return;
				}
				switch (mServerState)
				{
					case IPServerState.RequestingReload:
						mServerState = IPServerState.ReloadSuccess;
						break;
					case IPServerState.RequestingMatrices:
						if (e.Response.StartsWith("Child", StringComparison.Ordinal) || e.Response.StartsWith("Matrix", StringComparison.Ordinal))
						{
							int num = e.Response.IndexOf('=') + 1;
							mReturnedMatrixValid = Convert.ToInt32(e.Response.Substring(num, 1), CultureInfo.InvariantCulture) != 0;
							mReturnedMatrixHex = e.Response.Substring(num + 1);
							mMatrixDataReceived = true;
						}
						break;
				}
			}
		}

		private Process getProcess()
		{
			Process[] processesByName = Process.GetProcessesByName("railworks");
			if (processesByName.Length > 0)
			{
				return processesByName[0];
			}
			return null;
		}

		private void open()
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = Path.Combine(AppServices.ResourceManager.DeploymentPath, "RailWorks.exe");
			processStartInfo.Arguments = "start_up_mode=depot";
			if (HasVCArgument)
			{
				processStartInfo.Arguments += " -VC";
			}
			if (HasICArgument)
			{
				processStartInfo.Arguments += " -IC";
			}
			processStartInfo.UseShellExecute = false;
			processStartInfo.WorkingDirectory = AppServices.ResourceManager.DeploymentPath;
			Process.Start(processStartInfo);
		}

		private void reload()
		{
			if (mDocument != null)
			{
				if (!IsShapeViewerOpen)
				{
					open();
				}
				else
				{
					Process process = getProcess();
					if (process != null)
					{
						Native.SetWindowPos(process.MainWindowHandle, AppServices.MainWindow.Handle, 0, 0, 0, 0, 3u);
					}
				}
			}
			else if (IsShapeViewerOpen)
			{
				unloadShape();
			}
			mServerState = IPServerState.Disconnected;
		}

		public void Close()
		{
			if (IsShapeViewerOpen)
			{
				mIPC.SendMessage(new InterProcessMessage("AppClose"));
			}
		}

		private void reloadShapeMessage(uint timeout = 6000u)
		{
			mCurMessage = new InterProcessMessage("RLastBlueprintEditorShape", timeout);
			mIPC.SendMessage(mCurMessage);
		}

		private void unloadShape(uint timeout = 6000u)
		{
			mCurMessage = new InterProcessMessage("UnloadBlueprintEditorShape", timeout);
			mIPC.SendMessage(mCurMessage);
		}

		private void resetSharedMemState()
		{
			mIPC.ResetSharedMemoryState();
		}

		private void requestMatrix(string msg, int i, string origHex, uint timeout = 6000u)
		{
			mMatrixDataReceived = false;
			mCurMessage = new InterProcessMessage(string.Format(CultureInfo.InvariantCulture, msg + " {0} {1}", new object[2] { i, origHex }), timeout);
			mIPC.SendMessage(mCurMessage);
		}
	}
}
