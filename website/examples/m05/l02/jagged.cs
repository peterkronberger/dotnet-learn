// An array of arrays: the outer array has 3 slots, no row exists yet
int[][] rows = new int[3][];
Console.WriteLine(rows[0] == null);
rows[0] = new int[] { 1 };
rows[1] = new int[] { 2, 3 };
rows[2] = new int[] { 4, 5, 6 };
Console.WriteLine(rows.Rank);
for (int r = 0; r < rows.Length; r++)
{
    Console.WriteLine($"row {r} has {rows[r].Length} elements");
}
Console.WriteLine(rows[2][1]);
// An initializer creates the rows at once; each row needs its own new int[]
int[][] triangle =
{
    new int[] { 1 },
    new int[] { 1, 1 },
    new int[] { 1, 2, 1 },
};
Console.WriteLine(triangle[2][1]);
