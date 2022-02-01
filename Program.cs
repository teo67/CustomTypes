NodeMap.NodeMap<int> myMap = new NodeMap.NodeMap<int>();

myMap.Push(1);
myMap.Push(4);
myMap.Push(7);
myMap.Add(0, 2);
myMap.Add(0, 3);
myMap.Add(1, 5);
myMap.Add(1, 6);
myMap.Add(2, 8);
myMap.Add(2, 9);

Console.WriteLine(myMap);