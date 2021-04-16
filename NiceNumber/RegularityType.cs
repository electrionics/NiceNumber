namespace NiceNumber
{
    public enum RegularityType
    {
        SameDigitsSequential = 1, // 111 (1 1 1)
        SameDigitsWithFixedGap = 2, // 12121 (1 1 1, 2 2)
        SameDigitsAtAnyPosition = 3, // 1231 (1 1)
        
        AriphmeticProgressionSequential = 4, // 121314 (12 13 14)
        ArithmeticProgressionWithFixedGap = 5, // 142937 (1 2 3)
        ArithmeticProgressionAtAnyPosition = 6, // 123974 (1 2 3 4)
        
        MirrorNumbersSequential = 7, // 145541 (145 541)
        MirrorNumbersWithBetweenGap = 8, // 14579541 (145 541)
        MirrorNumbersWithFixedGap = 9, // 14659995141 (145 541) TODO: need gap #2 ?
        MirrorDigitsWithFixedGap = 10, // 12425757481 (145 541)
        MirrorDigitsAtAnyPosition = 11, // ...
        
        SameNumbersSequential = 12, //232323 (23 23 23)
        SameNumbersWithFixedGap = 13, //2377235523 (23 23 23)
        SameNumbersAtAnyPosition = 14, //239678023 (23 23)
    }
}