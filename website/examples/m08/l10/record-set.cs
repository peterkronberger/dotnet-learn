HashSet<ClassSeat> classSeats = [new ClassSeat(2, 5), new ClassSeat(2, 5)];
HashSet<RecordSeat> recordSeats = [new RecordSeat(2, 5), new RecordSeat(2, 5)];

Console.WriteLine(classSeats.Count);
Console.WriteLine(recordSeats.Count);

class ClassSeat(int row, int number)
{
    public int Row => row;
    public int Number => number;
}

record RecordSeat(int Row, int Number);
