<Query Kind="Program" />

class Animal { }
class Dog : Animal { }
class Cat : Animal { }

void Write (Animal[] a)
{
	a.Dump();
}

void Main()
{
	Write (new Animal[] { new Animal() } );	
	Write (new Cat[]    { new Cat()    } );	
	Write (new Dog[]    { new Dog()    } );	
}