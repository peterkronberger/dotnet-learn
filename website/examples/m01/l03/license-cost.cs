// 1. Set the inputs
decimal pricePerSeat = 4.99m;
int seats = 12;
decimal discountRate = 0.10m;

// 2. Use decimal for money, 3. calculate
decimal total = pricePerSeat * seats;
decimal discounted = total * (1 - discountRate);

// 4. Print the result
Console.WriteLine($"Total: {total}");
Console.WriteLine($"With discount: {discounted}");
