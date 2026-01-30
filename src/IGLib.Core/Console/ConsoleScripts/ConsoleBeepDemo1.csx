
// Source: https://gist.github.com/Twelve0fNine/9141812

using System;
using System.Diagnostics;
using System.Threading;

public class ProgramBeep
{


    public static void Main(params string[] args)
    {
        //Just a little Stopwatch ...
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        //Everything in this Region will be counted by the Stopwatch
        #region Time
        //SuperMario();
        //TetrisSound();
        GermanHym();
        #endregion
        //End of Time

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                            ts.Minutes,
                                            ts.Seconds,
                                            ts.Milliseconds / 10);
        Console.WriteLine("\n\nDuration of this Song was  " + elapsedTime);
        Console.ReadKey();
        Console.WriteLine("Press any Key to exit ...");


        //Some other Stuff you like to mess up with ...
        //System.Media.SystemSounds.Beep.Play();
        //System.Media.SystemSounds.Asterisk.Play();      
        //System.Media.SystemSounds.Exclamation.Play();


    }

    private static void GermanHym()
    {
        //The German Hymne

        Console.WriteLine("Something German ...");

        Console.Beep(704, 750);
        Console.Beep(792, 250);
        Console.Beep(880, 500);
        Console.Beep(792, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 250);
        Console.Beep(660, 250);
        Console.Beep(704, 500);
        Console.Beep(1188, 500);
        Console.Beep(1056, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 500);
        Console.Beep(880, 250);
        Console.Beep(704, 250);
        Console.Beep(1056, 1000);
        Console.Beep(704, 750);
        Console.Beep(792, 250);
        Console.Beep(880, 500);
        Console.Beep(792, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 250);
        Console.Beep(660, 250);
        Console.Beep(704, 500);
        Console.Beep(1188, 500);
        Console.Beep(1056, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 500);
        Console.Beep(880, 250);
        Console.Beep(704, 250);
        Console.Beep(1056, 1000);
        Console.Beep(792, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 250);
        Console.Beep(660, 250);
        Console.Beep(528, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 500);
        Console.Beep(792, 250);
        Console.Beep(660, 250);
        Console.Beep(528, 500);
        Console.Beep(1056, 500);
        Console.Beep(940, 500);
        Console.Beep(880, 750);
        Console.Beep(880, 250);
        Console.Beep(990, 500);
        Console.Beep(940, 250);
        Console.Beep(1056, 250);
        Console.Beep(1056, 1000);
        Console.Beep(1408, 750);
        Console.Beep(1320, 250);
        Console.Beep(1320, 250);
        Console.Beep(1188, 250);
        Console.Beep(1056, 500);
        Console.Beep(1188, 750);
        Console.Beep(1056, 250);
        Console.Beep(1056, 250);
        Console.Beep(940, 250);
        Console.Beep(880, 500);
        Console.Beep(792, 750);
        Console.Beep(880, 125);
        Console.Beep(940, 125);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(940, 250);
        Console.Beep(792, 250);
        Console.Beep(704, 500);
        Console.Beep(880, 250);
        Console.Beep(792, 250);
        Console.Beep(704, 1000);
        Console.Beep(1408, 750);
        Console.Beep(1320, 250);
        Console.Beep(1320, 250);
        Console.Beep(1188, 250);
        Console.Beep(1056, 500);
        Console.Beep(1188, 750);
        Console.Beep(1056, 250);
        Console.Beep(1056, 250);
        Console.Beep(940, 250);
        Console.Beep(880, 500);
        Console.Beep(792, 750);
        Console.Beep(880, 125);
        Console.Beep(940, 125);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(940, 250);
        Console.Beep(792, 250);
        Console.Beep(704, 500);
        Console.Beep(880, 250);
        Console.Beep(792, 250);
        Console.Beep(704, 1000);
    }

    private static void TetrisSound()
    {
        Console.WriteLine("Let's play Tetris ;)\nSorry, I mean let's listen to Tetris :-D\n Just about 1 Minute ...");

        Console.Beep(1320, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(1320, 125);
        Console.Beep(1188, 125);
        Console.Beep(1056, 250);
        Console.Beep(990, 250);
        Console.Beep(880, 500);
        Console.Beep(880, 250);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 750);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        System.Threading.Thread.Sleep(250);
        Console.Beep(1188, 500);
        Console.Beep(1408, 250);
        Console.Beep(1760, 500);
        Console.Beep(1584, 250);
        Console.Beep(1408, 250);
        Console.Beep(1320, 750);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        System.Threading.Thread.Sleep(500);
        Console.Beep(1320, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(1320, 125);
        Console.Beep(1188, 125);
        Console.Beep(1056, 250);
        Console.Beep(990, 250);
        Console.Beep(880, 500);
        Console.Beep(880, 250);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 750);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        System.Threading.Thread.Sleep(250);
        Console.Beep(1188, 500);
        Console.Beep(1408, 250);
        Console.Beep(1760, 500);
        Console.Beep(1584, 250);
        Console.Beep(1408, 250);
        Console.Beep(1320, 750);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        System.Threading.Thread.Sleep(500);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 1000);
        Console.Beep(440, 1000);
        Console.Beep(419, 1000);
        Console.Beep(495, 1000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 500);
        Console.Beep(660, 500);
        Console.Beep(880, 1000);
        Console.Beep(838, 2000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 1000);
        Console.Beep(440, 1000);
        Console.Beep(419, 1000);
        Console.Beep(495, 1000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 500);
        Console.Beep(660, 500);
        Console.Beep(880, 1000);
        Console.Beep(838, 2000);
    }

    private static void SuperMario()
    {
        Console.Beep(659, 125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(523, 125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(784, 125);
        Thread.Sleep(375);
        Console.Beep(392, 125);
        Thread.Sleep(375);
        Console.Beep(523, 125);
        Thread.Sleep(250);
        Console.Beep(392, 125);
        Thread.Sleep(250);
        Console.Beep(330, 125);
        Thread.Sleep(250);
        Console.Beep(440, 125);
        Thread.Sleep(125);
        Console.Beep(494, 125);
        Thread.Sleep(125);
        Console.Beep(466, 125);
        Thread.Sleep(42);
        Console.Beep(440, 125);
        Thread.Sleep(125);
        Console.Beep(392, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(784, 125);
        Thread.Sleep(125);
        Console.Beep(880, 125);
        Thread.Sleep(125);
        Console.Beep(698, 125);
        Console.Beep(784, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(587, 125);
        Console.Beep(494, 125);
        Thread.Sleep(125);
        Console.Beep(523, 125);
        Thread.Sleep(250);
        Console.Beep(392, 125);
        Thread.Sleep(250);
        Console.Beep(330, 125);
        Thread.Sleep(250);
        Console.Beep(440, 125);
        Thread.Sleep(125);
        Console.Beep(494, 125);
        Thread.Sleep(125);
        Console.Beep(466, 125);
        Thread.Sleep(42);
        Console.Beep(440, 125);
        Thread.Sleep(125);
        Console.Beep(392, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(784, 125);
        Thread.Sleep(125);
        Console.Beep(880, 125);
        Thread.Sleep(125);
        Console.Beep(698, 125);
        Console.Beep(784, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(587, 125);
        Console.Beep(494, 125);
        Thread.Sleep(375);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(415, 125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(698, 125);
        Thread.Sleep(125);
        Console.Beep(698, 125);
        Console.Beep(698, 125);
        Thread.Sleep(625);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(415, 125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(622, 125);
        Thread.Sleep(250);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(523, 125);
        Thread.Sleep(1125);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(415, 125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(698, 125);
        Thread.Sleep(125);
        Console.Beep(698, 125);
        Console.Beep(698, 125);
        Thread.Sleep(625);
        Console.Beep(784, 125);
        Console.Beep(740, 125);
        Console.Beep(698, 125);
        Thread.Sleep(42);
        Console.Beep(622, 125);
        Thread.Sleep(125);
        Console.Beep(659, 125);
        Thread.Sleep(167);
        Console.Beep(415, 125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Thread.Sleep(125);
        Console.Beep(440, 125);
        Console.Beep(523, 125);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(622, 125);
        Thread.Sleep(250);
        Console.Beep(587, 125);
        Thread.Sleep(250);
        Console.Beep(523, 125);
        Thread.Sleep(625);
    }



}
