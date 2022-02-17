<h1>Custom Data Structures for C# !</h1>
<p>To run, make sure you have the C# extension for VSCode installed and use "dotnet run".</p>
<h3>Current Datatypes</h3>
<ul>
    <li>Circle - nodes are arranged in a circle, with each one pointing in both the counterclockwise and clockwise directions. A custom Metadata class is included for keying nodes to strings for later use.</li>
    <li>NTree - like a binary tree, but with n connections per node instead of two. Because of this, the supplied comparator must be able to return integers from 0 to n - 1.</li>
    <li>NodeMap - A two-dimensional array of nodes, with each pointing down and to the right. This type is irrelevant to most projects, as inserting and deleting are high in time complexity and involve a lot of connection shifting.</li>
</ul>