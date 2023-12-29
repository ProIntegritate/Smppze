' Compile and start snooze.exe -h to get help

Module Module1

    Public cf As ConsoleColor = Console.ForegroundColor
    Public cb As ConsoleColor = Console.BackgroundColor
    Public bTerm As Boolean = False        ' Signal to termi for main() loop
    Public bBeep As Boolean = True         ' Do Beeps on start/stop/abort
    Public bHidden As Boolean = False      ' No console output

    Public bHideEndTime As Boolean = False
    Public bHideLocalTime As Boolean = False
    Public bHideUTCTime As Boolean = False


    Public Function fDoTheMath(ByVal sValue As String) As Double

        sValue = sValue.Replace(Chr(9), "").Replace(" ", "") ' No tabs or spaces.

        Dim sOp As String = ""
        If InStr(1, sValue, "+") Then sOp = "+"
        If InStr(1, sValue, "-") Then sOp = "-"
        If InStr(1, sValue, "*") Then sOp = "*"
        If InStr(1, sValue, "/") Then sOp = "/"

        If sOp.Trim = "" Then
            Try
                Return CInt(sValue) ' Exit with given value if no math operator found
            Catch ex As Exception
                Return 0 ' Obvi something failed
            End Try
        End If

        If Microsoft.VisualBasic.Left(sValue, 1) = "+" Then Return CDbl(sValue) ' Positive value, + operator is superflous here.
        If Microsoft.VisualBasic.Left(sValue, 1) = "-" Then Return CDbl(sValue) ' Negative value
        If Microsoft.VisualBasic.Left(sValue, 1) = "*" Then Return CDbl(0) ' Can't return this as value
        If Microsoft.VisualBasic.Left(sValue, 1) = "/" Then Return CDbl(0) ' Can't return this as value


        Dim sVals() As String = "".Split(",") ' Init

        sValue = sValue.Replace("+", " ") ' Make split char from operator
        sValue = sValue.Replace("-", " ")
        sValue = sValue.Replace("*", " ")
        sValue = sValue.Replace("/", " ")

        sVals = sValue.Split(" ") ' Use above split

        Dim iRetval As Double = 0 ' Double used by date.add*() functions

        Select Case sOp ' Calculate
            Case "+"
                iRetval = CInt(sVals(0)) + CInt(sVals(1))
            Case "-"
                iRetval = CInt(sVals(0)) - CInt(sVals(1))
            Case "*"
                iRetval = CInt(sVals(0)) * CInt(sVals(1))
            Case "/"
                iRetval = CInt(sVals(0)) / CInt(sVals(1))
        End Select

        If iRetval < 0 Then iRetval = 0 ' Can't wait negative time. That would be cool, but...
        Return iRetval

    End Function

    Sub ShowHelp()

        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine("Snooze, Written in 2022-2023 by Glenn Larsson.")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("")
        Console.WriteLine("Options: ")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine(" -h                        = Show this help text")
        Console.WriteLine(" -beep                     = Make start/stop/abort beeps (Default: True)")
        Console.WriteLine(" -ticks                    = Make a ticking sound every second (Default: False)")
        Console.WriteLine(" -hidden                   = Non verbose mode. No console output at all.")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("")
        Console.WriteLine("To hide specific things on screen:")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine(" -he                       = This will hide the Endtime from being presented on screen.")
        Console.WriteLine(" -hl                       = This will hide the Localtime from being presented on screen.")
        Console.WriteLine(" -hu                       = This will hide the UTCTime from being presented on screen.")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("")
        Console.WriteLine("To set a timespan, simply do:")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("> snooze.exe 10s           = this will pause for 10 seconds.")
        Console.WriteLine("> snooze.exe 5m            = this will pause for 5 minutes.")
        Console.WriteLine("> snooze.exe 3Y 6M         = this will pause for 3 years and 6 months.")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("")
        Console.WriteLine("To set a specific end time, do:")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("> snooze.exe -u <endtime>  = <endtime> is an ISODATE, like 2023-12-31T23:59:59.")
        Console.WriteLine("It also works with just a date/time operator:")
        Console.WriteLine("> snooze.exe -u 2023-12-31 = This will wait until the last day of the year 2023.")
        Console.WriteLine("> snooze.exe -u 23:59:59   = This will wait until the final second of the day.")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("")
        Console.WriteLine("Supported time units are s,m,h,D,W,M,Y (seconds, minutes,hours,Days,Weeks,Months,Years)")
        Console.WriteLine("You can also do math like: 30*2s -15s - that would add a wait period of 45 seconds. (30*2)-15")
        Console.ForegroundColor = ConsoleColor.White

        Environment.Exit(0)

    End Sub

    Sub Main()

        Dim bTicks As Boolean = False       '/ticks         - Play ticks during countdown, only useful in main()

        '/until <dateformat> - "yyyy-mm-ddThh:mm:ss.mmm" = dEnddate - date.now + any above variable added (end user problem if given)
        Dim dEndDate As Date = Date.Now '.AddSeconds(8).AddMilliseconds(950)
        Dim bGivenDate As Boolean = False ' ^ dEnddate flag

        ' Get CMD line variables and set globals
        Dim sCmdArgs() As String = Environment.GetCommandLineArgs
        If UBound(sCmdArgs) = 0 Then
            ShowHelp()
        End If



        For n = 1 To UBound(sCmdArgs)

            Select Case (sCmdArgs(n))
                Case "-u" ' Until localtime
                    dEndDate = Date.Parse(sCmdArgs(n + 1))
                Case "-h"
                    ShowHelp()
                Case "-hidden"
                    bHidden = True
                Case "-ticks"
                    bTicks = True
                Case "-beep"
                    bBeep = False
                Case "-he"
                    bHideEndTime = True
                Case "-hl"
                    bHideLocalTime = True
                Case "-hu"
                    bHideUTCTime = True
                Case Else

                    ' Seconds
                    If InStr(1, sCmdArgs(n), "s") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("s", "")
                        dEndDate = dEndDate.AddSeconds(fDoTheMath(sCmdArgs(n)))
                    End If

                    ' Minutes
                    If InStr(1, sCmdArgs(n), "m") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("m", "")
                        dEndDate = dEndDate.AddMinutes(fDoTheMath(sCmdArgs(n)))
                    End If

                    ' Hours
                    If InStr(1, sCmdArgs(n), "h") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("h", "")
                        dEndDate = dEndDate.AddHours(fDoTheMath(sCmdArgs(n)))
                    End If

                    ' Days
                    If InStr(1, sCmdArgs(n), "D") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("D", "")
                        dEndDate = dEndDate.AddDays(fDoTheMath(sCmdArgs(n)))
                    End If

                    ' Weeks
                    If InStr(1, sCmdArgs(n), "W") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("W", "")
                        dEndDate = dEndDate.AddDays(fDoTheMath(sCmdArgs(n)) * 7)
                    End If

                    ' Months
                    If InStr(1, sCmdArgs(n), "M") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("M", "")
                        dEndDate = dEndDate.AddMonths(fDoTheMath(sCmdArgs(n)))
                    End If

                    ' Years
                    If InStr(1, sCmdArgs(n), "Y") Then
                        sCmdArgs(n) = sCmdArgs(n).Replace("Y", "")
                        dEndDate = dEndDate.AddYears(fDoTheMath(sCmdArgs(n)))
                    End If

            End Select
        Next

        ' Thread: Wait for key input, cant do that in Main()
        Dim thAbort As Threading.Thread = New Threading.Thread(AddressOf threadAbort)
        thAbort.Start()

        If bBeep = True Then Console.Beep(1200, 50) ' Beep @ start if set

        Dim sLastSecond As String = Now.Second.ToString ' Just for Ticks!

        Console.Clear()

        Dim bKeepRunning As Boolean = True
        While (bKeepRunning) ' Main loop


            If bHidden = False Then
                Try
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.Cyan
                    Console.WriteLine("   Timeleft: " & (dEndDate - Date.Now).ToString("G") & " ")
                    Console.ForegroundColor = ConsoleColor.Green
                    If bHideEndTime = False Then Console.WriteLine("    Endtime: " & dEndDate.ToString("G") & " ")
                    Console.ForegroundColor = ConsoleColor.Yellow
                    If bHideLocalTime = False Then Console.WriteLine(" LOCAL Time: " & Date.Now.ToString("G") & " ")
                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    If bHideUTCTime = False Then Console.WriteLine("   UTC Time: " & Date.UtcNow.ToString("G") & " ")
                Catch ex As Exception
                End Try
            End If

            Threading.Thread.Sleep(47) ' Sleep some before updating screen again. Lowers CPU usage significantly.

            '  Beep once a second if ticks set to True
            If sLastSecond <> Now.Second.ToString Then ' Play beep if new second
                sLastSecond = Now.Second.ToString
                If bTicks = True Then Console.Beep(900, 25) ' A brief second tick, sounds like a metronome
            End If

            ' Step back to previous line
            If bHidden = False Then
                Try
                    'Console.CursorTop = Console.CursorTop - 1 ' Go back to prev line
                    Console.CursorTop = 0
                    Console.CursorLeft = 0  ' Go back to prev line
                Catch ex As Exception
                End Try
            End If

            If dEndDate <= Date.Now Then bKeepRunning = False ' Exit when done
            If bTerm = True Then
                bKeepRunning = False ' Should we exit?
                bTicks = False ' No more ticks
                bBeep = False ' No exit beeps (good ones that is since we're aborting)
            End If

        End While

        Console.ForegroundColor = cf ' Restore console colors
        Console.BackgroundColor = cb

        If bHidden = False And bTerm = False Then
            Console.Clear()
            Console.WriteLine("Done!")
        End If

        If bBeep = True Then ' Do final beeps
            Console.Beep(1200, 50)
            Console.Beep(1200, 50)
        End If

        Environment.Exit(0) ' This really needs to be here.

    End Sub

    Sub threadAbort()

        Dim cki As ConsoleKeyInfo = Console.ReadKey() ' Wait endlessly for input

        bTerm = True ' Signal term to main() loop so it doesn't continue

        If bBeep = True Then ' Do abortion beeps if...
            Console.Beep(1200, 50)
            Console.Beep(800, 100)
            Console.Beep(400, 100)
            Threading.Thread.Sleep(50)
        End If

        Threading.Thread.Sleep(100)

        Console.ForegroundColor = cf ' Restore console colors
        Console.BackgroundColor = cb

        If bHidden = False Then
            Console.CursorLeft = 0
            Console.Clear()
            Console.WriteLine("Aborted!")
        End If
        Environment.Exit(0)

    End Sub

End Module
