.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method public static void ParseCalcArguments(string[] args,
        [out] float64& val1,
        [out] valuetype Hw2.CalculatorOperation& operation,
        [out] float64& val2) cil managed
     {
        // init by default val1
        ldarg.1
        ldc.r8 0.0
        stind.r8
        
        // init by default val2
        ldarg.3
        ldc.r8 0.0
        stind.r8
        
        // init by default operation
        ldarg.2
        ldc.i4.s 4
        stind.i1
        
        // transfer control to AssertLength(string[] args)
        ldarg.0
        call void Hw2.Parser::AssertLength(string[])
        
        .try
        {
            // Parse first element from args into val1
            ldarg.1
            ldarg.0
            ldc.i4.0
            ldelem.ref
            call float64 [System.Runtime]System.Double::Parse(string)
            stind.r8
            
            // Parse second element from args into operation
            ldarg.2
            ldarg.0
            ldc.i4.1
            ldelem.ref
            call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
            stind.i1
            
            // Parse third element from args into val2
            ldarg.3
            ldarg.0
            ldc.i4.2
            ldelem.ref
            call float64 [System.Runtime]System.Double::Parse(string)
            stind.r8
            
            leave.s Return
        }
        catch [System.Runtime]System.FormatException
        {
            ldstr "Could not convert given value to a number"
            newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
            throw
        }
        
        Return:
        ret
     }   
     
     .method private static void AssertLength(string[] args) cil managed
     {
        // null check
        ldarg.0
        brtrue.s CheckLength
        
        ldstr "args"
        newobj instance void [System.Runtime]System.ArgumentNullException::.ctor(string)
        throw 
                
        // length check
        CheckLength:
        ldarg.0
        ldlen
        conv.i4
        ldc.i4.3
        ceq
        brtrue.s Return
        
        ldstr "Expression must contain 2 values and 1 operation"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
        
        Return:
        ret
     }
     
     .method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
     {
        // If arg equals to plus
        ldarg.0
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s EqualsToPlus
                
        // If arg equals to minus
        ldarg.0
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s EqualsToMinus
                
        // If arg equals to multiply
        ldarg.0
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s EqualsToMultiply
                
        // If arg equals to divide
        ldarg.0
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s EqualsToDivision       
        
        br.s Exception       
        
        EqualsToPlus:
        ldc.i4.0
        ret
        
        EqualsToMinus:
        ldc.i4.1
        ret
        
        EqualsToMultiply:
        ldc.i4.2
        ret

        EqualsToDivision:
        ldc.i4.3
        ret
       
        // If arg does not equal to any operations
        Exception:
        ldstr "Could not convert given value to an operation"
        newobj instance void[System.Runtime]System.InvalidOperationException::.ctor(string)
        throw
     }
}
