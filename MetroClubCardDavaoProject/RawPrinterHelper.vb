Imports System
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Public Class RawPrinterHelper

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
    Public Class DOCINFOA
        <MarshalAs(UnmanagedType.LPStr)>
        Public pDocName As String
        <MarshalAs(UnmanagedType.LPStr)>
        Public pOutputFile As String
        <MarshalAs(UnmanagedType.LPStr)>
        Public pDataType As String
    End Class

    <DllImport("winspool.Drv", EntryPoint:="OpenPrinterA", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Public Shared Function OpenPrinter(ByVal szPrinter As String, ByRef hPrinter As IntPtr, ByVal pd As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="ClosePrinter", SetLastError:=True)>
    Public Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="StartDocPrinterA", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Public Shared Function StartDocPrinter(ByVal hPrinter As IntPtr, ByVal Level As Integer, ByVal di As DOCINFOA) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="EndDocPrinter", SetLastError:=True)>
    Public Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="StartPagePrinter", SetLastError:=True)>
    Public Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="EndPagePrinter", SetLastError:=True)>
    Public Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="WritePrinter", SetLastError:=True)>
    Public Shared Function WritePrinter(ByVal hPrinter As IntPtr, ByVal pBytes As IntPtr, ByVal dwCount As Integer, ByRef dwWritten As Integer) As Boolean
    End Function

    Public Shared Function SendBytesToPrinter(ByVal szPrinterName As String, ByVal bytes As Byte()) As Boolean
        Dim hPrinter As IntPtr = IntPtr.Zero
        Dim dwWritten As Integer = 0
        Dim di As New DOCINFOA()
        di.pDocName = "Raw ESC/POS"
        di.pDataType = "RAW"

        If OpenPrinter(szPrinterName, hPrinter, IntPtr.Zero) Then
            If StartDocPrinter(hPrinter, 1, di) Then
                If StartPagePrinter(hPrinter) Then

                    Dim pUnmanagedBytes As IntPtr = Marshal.AllocCoTaskMem(bytes.Length)
                    Marshal.Copy(bytes, 0, pUnmanagedBytes, bytes.Length)

                    WritePrinter(hPrinter, pUnmanagedBytes, bytes.Length, dwWritten)

                    Marshal.FreeCoTaskMem(pUnmanagedBytes)

                    EndPagePrinter(hPrinter)
                End If
                EndDocPrinter(hPrinter)
            End If
            ClosePrinter(hPrinter)
        End If

        Return True
    End Function

End Class