LINQ Performance Example
==============

This is a simple example to illustrate the performance advantages of converting a List to a Dictionary when doing multiple lookups within a loop.

It's easy to forget the time complexity of LINQ methods. It's not uncommon to see LINQ methods like Single or First used within a loop to select items from a List. Depending on the circumstances, it may make sense to convert the List to a Dictionary and take advantage of the Dictionary's O(1) lookup time complexity.
