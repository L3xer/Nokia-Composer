module ParsingTests

open NUnit.Framework
open Parsing

[<TestFixture>]
type ``when parsing a score`` ()=

    [<Test>]
    member this.``it should parse a simple score`` ()=
        let score = "32.#d3 16-"
        let result = parse score
        let assertFirstToken token =
            Assert.That({fraction=Thirtyseconth;extended=true}, Is.EqualTo(token.length))
            Assert.That(Tone(DSharp,Three), Is.EqualTo(token.sound))
        let assertSecondToken {length=length; sound=sound} =
            Assert.That({fraction=Sixteenth;extended=false}, Is.EqualTo(length))
            Assert.That(sound, Is.EqualTo(Rest)) 
            
        match result with
            | Choice2Of2 errorMsg -> Assert.Fail(errorMsg)
            | Choice1Of2 tokens ->
                Assert.That(2, Is.EqualTo(List.length tokens))
                List.head tokens |> assertFirstToken
                List.item 1 tokens |> assertSecondToken           
