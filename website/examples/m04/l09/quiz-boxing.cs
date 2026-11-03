int level = 1;
object box = level;
level++;
int unboxed = (int)box;
level = level + unboxed;
Console.WriteLine(level);
Console.WriteLine(box);
