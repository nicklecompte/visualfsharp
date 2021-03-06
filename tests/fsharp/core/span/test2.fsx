#r @"..\..\..\..\packages\System.Memory.4.5.0-rc1\lib\netstandard2.0\System.Memory.dll"
#r @"..\..\..\..\packages\NETStandard.Library.NETFramework.2.0.0-preview2-25405-01\build\net461\ref\netstandard.dll"

#nowarn "9"
#nowarn "51"

namespace System.Runtime.CompilerServices

    open System
    open System.Runtime.CompilerServices
    open System.Runtime.InteropServices
    [<AttributeUsage(AttributeTargets.All,AllowMultiple=false)>]
    [<Sealed>]
    type IsReadOnlyAttribute() =
        inherit System.Attribute()

    [<AttributeUsage(AttributeTargets.All,AllowMultiple=false)>]
    [<Sealed>]
    type IsByRefLikeAttribute() =
        inherit System.Attribute()

namespace Tests
    open System
    open System.Runtime.CompilerServices
    open System.Runtime.InteropServices
    open FSharp.NativeInterop

    module Tests =
        let mutable beef = 1

        let returning_span (x: byref<int>) = Span.Empty

        let returning_ref (x: byref<int>) = &x

        let returning_ref2 (x: byref<int>) (y: byref<int>) = &x

        let passing_span_returning_span (x: Span<int>) = Span.Empty

        let passing_span_returnin_ref (x: Span<int>) = &x.[0]

        let passing_span2 (x: Span<int>) (y: Span<int>) = ()

        let passing_byref_of_span_returning_span (x: byref<Span<int>>) = x

        let passing_inref_of_span_returning_span (x: inref<Span<int>>) = x

        let passing_byref_of_span_returning_byref_of_span (x: byref<Span<int>>) = &x

        let passing_byref_of_span2_returning_byref_of_span (x: byref<Span<int>>) (y: byref<Span<int>>) = &y

        let passing_byref_of_span_and_span_returning_byref_of_span (x: byref<Span<int>>) (y: Span<int>) = 
            let y = &x
            &y

#if NOT_YET

        [<IsByRefLike;Struct>]
        type StructRecordRefLike =
            {
                mutable value: Span<int>
            }

            member this.Set(data: Span<int>) =
                this.value <- data

        [<IsByRefLike;Struct>]
        type StructRefLike =

            val mutable value: Span<int>

            new (value) = { value = value }

#endif

#if NEGATIVE

        let should_not_work () =
            let test = ReadOnlySpan.Empty
            let f = fun () -> test
            ()

        let should_not_work1 () =
            let mutable x = 1
            &x

        let should_not_work2 () =
            let mutable x = 1
            let mutable x = (let mutable y = &x in &y)

            let y = &returning_ref &x
            &y

        let should_not_work3 () =
            let mutable x = 1
            let mutable x =
                match &x with
                | x -> &x

            let y = &returning_ref &x
            &y

        let should_not_work4 () =
            let mutable x = 1
            let mutable y = &x
            y <- 1
            &y

        let should_not_work5 () =
            let x = 1
            let y = &x
            &y

        let should_not_work6 () =
            let mutable x = 1
            &returning_ref &x

        let should_not_work7 () =
            let mutable x = 1
            &returning_ref2 &beef &x

        let should_not_work8 () =
            let mutable x = 1
            let y = &returning_ref2 &beef &x
            &y

        let should_not_work9 () =
            let mutable s = Span.Empty
            passing_span_returning_span s

        let should_not_work10 () =
            let mutable s = Span.Empty
            &passing_span_returnin_ref s

        let should_not_work11 () =
            let mutable s = Span.Empty
            &passing_span_returnin_ref (passing_span_returning_span s)

        let should_not_work12 () =
            let mutable s = Span.Empty
            passing_byref_of_span_returning_span &s

        let should_not_work13 () =
            let mutable s = Span.Empty
            s

        let should_not_work14 () =
            let mutable s = Span.Empty
            let mutable x = &s
            passing_byref_of_span_returning_span &x

        let should_not_work15 (data: byref<Span<int>>) =
            let mutable stackData = Span.Empty
            let y = passing_byref_of_span2_returning_byref_of_span &stackData &data
            y

        let should_not_work16 (data: byref<Span<int>>) =
            let mutable stackData = Span.Empty
            passing_byref_of_span_and_span_returning_byref_of_span &stackData data

        let should_not_work17 (data: byref<Span<int>>) =
            let mutable stackData = Span.Empty
            let y = &passing_byref_of_span_and_span_returning_byref_of_span &stackData data
            y

        let should_not_work18 () =
            let mutable s = Span.Empty
            &passing_span_returnin_ref (passing_span_returning_span s)

#if NOT_YET

        let should_not_work19 (data: byref<StructRecordRefLike>) =
            let mutable stackData = Span.Empty
            data.value <- stackData

        let should_not_work20 () =
            let mutable data = Span.Empty
            let beef = { value = data }
            beef

        let should_not_work21 () =
            let mutable data = Span.Empty
            let beef = StructRefLike(data)
            beef

        let should_not_work22 (data: byref<StructRecordRefLike>) =
            let mutable x = Span.Empty
            data.Set(x)

#endif

        let should_not_work33 (x: int) = &x

        let should_not_work34 (x: int) =
            let y = &x
            &y

#endif

        let should_work1 () =
            Span.Empty

        let should_work3 () =
            let mutable beef = 1
            let y = &returning_ref &beef
            y
            
        let should_work2 () =
            let x = Span.Empty
            x

        let should_work4 () =
            &returning_ref2 &beef &beef

        let should_work5 () =
            let mutable x = 1
            returning_ref2 &x &x

        let should_work6 () =
            let mutable x = 1
            returning_span &x

        let should_work7 () =
            let mutable x = 1
            let y = returning_span &x
            y

        let should_work8 () =
            let s = Span.Empty
            passing_span_returning_span s

        let should_work9 () =
            let mutable s = Span.Empty
            passing_span_returnin_ref s

        let should_work10 () =
            let mutable s = Span.Empty
            passing_span_returnin_ref (passing_span_returning_span s)

        let should_work11 () =
            let s = Span.Empty
            &passing_span_returnin_ref (passing_span_returning_span s)

        let should_work12 () =
            let s = Span.Empty
            passing_inref_of_span_returning_span &s

        let should_work13 () =
            let s = Span.Empty
            let mutable x = &s
            passing_inref_of_span_returning_span &x

        let should_work14 (s: byref<Span<int>>) =
            passing_byref_of_span_returning_byref_of_span &s

        let should_work15 (s: byref<Span<int>>) =
            let x = &s
            let y = &passing_byref_of_span_returning_byref_of_span &x
            &y

        type TestDelegate = delegate of byref<Span<int>> -> Span<int>

        let should_work16 (s: byref<Span<int>>) =
            let f = TestDelegate(fun _ -> Span.Empty)
            f.Invoke(&s)

        type IBeef =

            abstract Test : byref<Span<int>> -> byref<Span<int>>

        let should_work17 () =
            { new IBeef with
                member __.Test s = &s
            }

        let should_work18 (data: byref<Span<int>>) =
            let y = passing_byref_of_span_and_span_returning_byref_of_span &data data
            y

        let should_work19 (data: byref<Span<int>>) =
            &passing_byref_of_span_and_span_returning_byref_of_span &data data

#if NOT_YET

        let should_work20 () =
            let data = Span.Empty
            let beef = StructRefLike(data)
            beef

        let should_work21 () =
            let data = Span.Empty
            let beef = { value = data }
            beef

        let should_work22 (data: byref<StructRecordRefLike>) =
            let s = Span.Empty
            data.value <- s

        let should_work23 (data: byref<StructRecordRefLike>) =
            let x = Span.Empty
            data.Set(x)

        let should_work24 () =
            let mutable s = Span.Empty
            let mutable srl = StructRefLike()
            let addr = &srl
            addr.value <- s

        let should_work25 () =
            let mutable s = Span.Empty
            let mutable srl = StructRefLike()
            srl.value <- s

        let should_work26 (data: byref<StructRefLike>) =
            let mutable s = Span.Empty
            let mutable srl = data
            srl.value <- s

#endif

        let should_work27 (s: Span<int>) = &s.[0]

        let should_work28 (s: Span<int>) =
            let y = &s.[0]
            &y