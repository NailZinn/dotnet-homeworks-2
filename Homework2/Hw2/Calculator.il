.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method public static float64 Calculate(
        float64 val1, 
        valuetype [Hw2]Hw2.CalculatorOperation operation,
        float64 val2) cil managed
    {
        // if operation is addition
        ldarg.1
        conv.i4
        ldc.i4.0
        ceq
        brtrue.s Add
        
        // if operation is substraction
        ldarg.1
        conv.i4
        ldc.i4.1
        ceq
        brtrue.s Sub
        
        // if operation is multiplication
        ldarg.1
        conv.i4
        ldc.i4.2
        ceq
        brtrue.s Mul
        
        // if operation is division
        ldarg.1
        conv.i4
        ldc.i4.3
        ceq
        brtrue.s Div
        
        br.s Exception
        
        Add:
        ldarg.0
        ldarg.2
        add
        ret
        
        Sub:
        ldarg.0
        ldarg.2
        sub
        ret
        
        Mul:
        ldarg.0
        ldarg.2
        mul
        ret
        
        Div:
        ldarg.0
        ldarg.2
        div
        ret
        
        Exception:
        newobj instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
        throw
    }
}
