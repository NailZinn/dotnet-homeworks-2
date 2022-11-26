namespace Hw5

type Message =
    | SuccessfulExecution = 0
    | WrongArgLength = 1
    | WrongArgFormatForValue1 = 2
    | WrongArgFormatForValue2 = 3
    | WrongArgFormatOperation = 4
    | DivideByZero = 5