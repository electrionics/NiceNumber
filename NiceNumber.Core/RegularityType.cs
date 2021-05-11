namespace NiceNumber.Core
{
    public enum RegularityType
    {
        SameDigits = 1, // seq 111 (1 1 1), gap 12121 (1 1 1, 2 2), gen 1231 (1 1)
        SameNumbers = 2, // seq 232323 (23 23 23), gap 2377235523 (23 23 23), gen 239678023 (23 23)
        MirrorDigits = 3, // seq 145541, gap 12425757481 (145 541), gen ...
        MultiplesNumbers = 4, 
        ArithmeticProgression = 5, // seq 121314 (12 13 14), gap 142937 (1 2 3), gen 123974 (1 2 3 4)
        GeometricProgression = 6 // seq ..., gap ..., gen ...
    }
}