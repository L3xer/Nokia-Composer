﻿module WavePackerTests

open NUnit.Framework
open WavePacker
open SignalGenerator
open System.IO

[<TestFixture>]
type ``When packing an audio file`` ()=
    let getFile miliseconds =
        generateSamples miliseconds 440.
        |> Array.ofSeq 
        |> pack
        |> (fun ms -> 
                ms.Seek(0L, SeekOrigin.Begin) |> ignore
                ms)

    [<Test>]
    member this.``the stream should start with 'RIFF'`` ()=
        let file = getFile 2000.
        let bucket = Array.zeroCreate 4
        file.Read(bucket, 0, 4) |> ignore
        let first4Chars = System.Text.Encoding.ASCII.GetString(bucket)
        Assert.That(first4Chars, Is.EqualTo("RIFF"))

    [<Test>]
    member this.``file size is correct`` ()=
        let formatOverhead = 44.
        let audioLenghts = [2000.; 50.; 1500.; 3000. ]
        let files = List.zip audioLenghts (List.map getFile audioLenghts)
        let assertLength (lenght, file:MemoryStream) =
            Assert.That((lenght/1000.) * 44100. * 2. + formatOverhead, Is.EqualTo(file.Length))
        List.iter assertLength files
