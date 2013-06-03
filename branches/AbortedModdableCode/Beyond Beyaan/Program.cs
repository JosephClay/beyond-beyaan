using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Beyond_Beyaan
{

	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
//=======================================================================================
// the code here is 32-bit only
// since all those handles returned by winapi functions will be used by value only and will never be dereferenced
// they are declared as Int32, not IntPtr for the sake of ease
		private const string MutexNoDupeName="BeyondBeyaanNoDupeMutex";
		private const string SharedMemName="BeyondBeyaanSharedData";
		
		//ERROR_ALREADY_EXISTS==0xB7
		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		static extern Int32 CreateMutex(Int32 attr, Int32 init_own, string name);
		
		[DllImport("kernel32.dll")]
		static extern Int32 GetLastError();
		
		[DllImport("kernel32.dll")]
		static extern Int32 ReleaseMutex(Int32 hmutex);
		
		[DllImport("kernel32.dll")]
		static extern Int32 CloseHandle(Int32 handle);
		
		[DllImport("user32.dll")]
		static extern bool SetForegroundWindow(IntPtr hwnd);

		//PAGE_READWRITE==0x04		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		static extern Int32 CreateFileMapping(Int32 hfile, Int32 secattr, UInt32 access, UInt32 szhi, UInt32 szlo, string name);

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		static extern Int32 OpenFileMapping(Int32 access, bool inherithandle, string name);
		
		[DllImport("kernel32.dll")]
		static extern Int32 GetCurrentProcessId();

		//FILE_MAP_WRITE==0x02
		//FILE_MAP_READ==0x04
		[DllImport("kernel32.dll")]
		static extern Int32 MapViewOfFile(Int32 hfile, Int32 access, UInt32 offsethi, UInt32 offsetlo, UInt32 mapsz);
		
		[DllImport("kernel32.dll")]
		static extern void CopyMemory(Int32 dst, Int32 src, UInt32 sz);
		
		[DllImport("kernel32.dll")]
		static extern bool UnmapViewOfFile(Int32 addr);
//!======================================================================================

		[STAThread]
		public static void Main()
		{
//=======================================================================================
// the code here is 32-bit only
// i suppose switching from Int32 to IntPtr will help to resolve that. need to investigate further
			Int32 hmutex;
			Int32 hmem;
			Int32 buf;
			Int32 pid;

			// create named mutex and request ownership, if no ownership received - there is another instance running
			if((hmutex=CreateMutex(0, 1, MutexNoDupeName))==0){
   				MessageBox.Show("Ion disturbance of unknown origin prevents from running Beyond Beyaan", "Science station reports:");
   				return;
			}
			// the only error condition possible if named mutex was created is that it has existed before a call to CreateMutex()
			if(GetLastError()==0xB7){ // there is another instance of the same process running already - find, show, die
				if((hmem=OpenFileMapping(0x04, false, SharedMemName))==0){ // failed to open shared memory object - just die
					CloseHandle(hmutex);
	   				MessageBox.Show("The debris found prevents from running Beyond Beyaan", "Science station reports:");
	   				return;
				}
				if((buf=MapViewOfFile(hmem, 0x04, 0, 0, 0))==0){ // failed to map shared memory - quite fatal
					CloseHandle(hmem);
					CloseHandle(hmutex);
	   				MessageBox.Show("Ion disturbance of unknown origin prevents from running Beyond Beyaan", "Science station reports:");
	   				return;
				}
				// now it's safe.. or is it?
				unsafe{CopyMemory((Int32)(&pid), buf, sizeof(Int32));}
				using(Process FirstProcess=Process.GetProcessById(pid)){
						SetForegroundWindow(FirstProcess.MainWindowHandle);
				}
				UnmapViewOfFile(buf);
				CloseHandle(hmem);
				CloseHandle(hmutex);
				return;
			}
			else{ // there shouldn't be any other errors if CreateMutex returned a valid handle
				if((hmem=CreateFileMapping(-1, 0, 0x04, 0, 4, SharedMemName))==0){ // failed to create shared memory object - consider dying
					CloseHandle(hmutex);
	   				MessageBox.Show("Ion disturbance of unknown origin prevents from running Beyond Beyaan", "Science station reports:");
	   				return;
				}
				if((buf=MapViewOfFile(hmem, 0x02, 0, 0, 0))==0){ // failed to map shared memory - quite fatal
					CloseHandle(hmem);
					CloseHandle(hmutex);
	   				MessageBox.Show("Ion disturbance of unknown origin prevents from running Beyond Beyaan", "Science station reports:");
	   				return;
				}
				// now it's safe.. again..
				pid=GetCurrentProcessId();
				unsafe{CopyMemory(buf, (Int32)(&pid), sizeof(Int32));}
			}
			
//!======================================================================================
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new BeyondBeyaan());
//=======================================================================================
			UnmapViewOfFile(buf);
			CloseHandle(hmem);
			ReleaseMutex(hmutex);
			CloseHandle(hmutex);
//!======================================================================================
		}
	}
}
