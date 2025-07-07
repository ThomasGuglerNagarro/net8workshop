using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1;

/*
internal class UnsafeAccessorSampleNOTWorkingBecauseRuntimeFeature
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name="PrivateField")]
    public extern static ref int GetSetPrivateField(SampleClassForUnsafeAccessor c);
}
*/
public class SampleClassForUnsafeAccessor
{
    private int privateField = 42;

    public int GetPrivateField()
    {
        return privateField;
    }

    public void SetPrivateField(int value)
    {
        privateField = value;
    }
}
