Public Class BitManipulation

    Shared Function ClearBit(MyByte, MyBit) As Int32
        Dim BitMask As Int32
        BitMask = 2 ^ MyBit
        Return MyByte And Not BitMask
    End Function

    Shared Function ExamineBit(MyByte, MyBit) As Boolean
        Dim BitMask As Int32
        BitMask = 2 ^ MyBit
        ExamineBit = ((MyByte And BitMask) > 0)
    End Function

    Shared Function SetBit(MyByte, MyBit) As Int32
        Dim BitMask As Int32
        BitMask = 2 ^ MyBit
        Return MyByte Or BitMask
    End Function

    Shared Function ToggleBit(MyByte, MyBit) As Int32
        Dim BitMask As Int32
        BitMask = 2 ^ MyBit
        Return MyByte Xor BitMask
    End Function
End Class
