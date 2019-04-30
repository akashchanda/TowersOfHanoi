/*
* Author: Akash Chanda
* Description: Towers of Hanoi simulation with OOP and dynamic number of rings
* Last modified: 04/29/2019
* To-do: stage 1: make Ring an object as well instead of int, make stack of type Ring (4/30)
*        stage 2: create thread for processing of movements, add debug statements (5/1)
*        stage 3: make UI with start and cancel buttons, refreshing every 120 ms showing updates (5/2-5/3)
*/

using System;
using System.Collections;

class Program
{
    static int moves = 0;
    static void Main(string[] args)
    {
        Tower firstTower = new Tower();
        Tower secondTower = new Tower();
        Tower thirdTower = new Tower();
        int numRings = args[0];
        for(int i = numRings; i > 0; i--) {
            firstTower.addRing(i);
        }
        
        //print towers after moving
        Console.Write("Towers before moving:\n");
        Console.Write("First Tower in reverse order: ");
        PrintTower(firstTower);
        Console.Write("Second Tower in reverse order: ");
        PrintTower(secondTower);
        Console.Write("Third Tower in reverse order: ");
        PrintTower(thirdTower);
        Console.Write("Number of moves = " + moves + "\n\n");
        
        //create TowerManager object to manage towers and their respective rings
        TowerManager towerManager = new TowerManager();
        //print towers after moving
        towerManager.MoveTowers(firstTower, secondTower, numRings, thirdTower);
        Console.Write("Towers after moving:\n");
        Console.Write("First Tower in reverse order: ");
        PrintTower(firstTower);
        Console.Write("Second Tower in reverse order: ");
        PrintTower(secondTower);
        Console.Write("Third Tower in reverse order: ");
        PrintTower(thirdTower);
        Console.Write("Number of moves = " + moves + "\n");
    }
}

class Tower 
{
    private Stack stack;
    private int numRings;

    //this constructor is used when the rings to be placed on it are unknown or there are no rings on it
    public Tower()
    {
        stack = new Stack();
        numRings = 0;
    }

    //this constructor is used when a predefined set of rings is to be used
    public Tower(int[] rings) {
        //create the tower
        stack = new Stack();
        //put the rings on it
        for(int i = rings.length - 1; i > 0; i--) {
            stack.push(rings[i]);
        }
        
    }
       
    //add a ring to the top of the tower if smaller then preceding ring
    bool AddRing(int ringValue) {
        if(TowerManager.CheckMoveValidity(this, ringValue))
        {
            stack.push(ringValue);
            return true;
        }
        //move was invalid so ring cannot be moved
        else return false;
    }   
        
    //ONLY for debugging. Prints out the contents of the stack while preserving them
    void PrintTower(Stack tower)
    {
        if(tower.Count == 0)
        {
            Console.Write("Tower empty\n");
        }
        else 
        {
            foreach(int ring in tower) 
            {
                Console.Write(ring + " ");
            }
            Console.Write("\n");
        }
    }
}

class TowerManager
{
    /*
    * Method description:
    * move rings from one stack (tower) to another recursively by moving every ring above the lowest one
    * to the auxillary tower and then moving the lowest ring to the final the destination. The process then
    * repeats with the new (shorter) tower having all but its lowermost ring moved to the destination. This 
    * continues until there is only one ring remaining
    * Inputs:
    * from and to are the towers from which the rings are being moved in order from and to, respectively
    * n is the number of rings to be moved in order
    * auxillary is the helper stack that is used to supplement movement
    * 
    * Output: from and to stacks are modified to reflect the move
    */
    void MoveTowers(Stack from, Stack to, int n, Stack auxillary) 
    {
        //base case: there's only one ring to move
        if(n == 1) 
        {
            MoveRing(from, to);
            return;
        }
        //move all rings above lowest one to auxillary tower. To is now the auxillary tower
        MoveTowers(from, auxillary, n - 1, to);
        //move lowest ring to destination tower
        MoveRing(from, to);
        //move smaller tower to destination tower atop larger ring. From becomes the auxillary tower
        MoveTowers(auxillary, to, n - 1, from);
    }
    
    /*
    * Method description: 
    * moves a ring from one tower (stack) to another
    * Inputs:
    * from and to are the stacks the ring is moving from and to respectively
    * 
    * Output: from and to stacks are modified to reflect the move
    */
    void MoveRing(Stack from, Stack to)
    {
        //remove the ring from the first stack if not empty
        if(from.Count == 0) return;
        int ringValue = from.Peek());
        if(TowerManager.CheckMoveValidity(to, ringValue))
        {
            to.Push(from.Pop());
        }
        //simulate move by pushing to the destination stack the popped value from the source stack
        moves++;
    }
    
    /*
    * Method description: 
    * checks whether a given move is valid given the ring below the ring to be added. 
    * This enforces the rule of disallowing the placement of a larger ring atop a smaller one.
    * Input:
    * stack is the stack to which the ring is supposed to be moved
    * ringValue is the value of the new ring to be placed on top 
    * Output: true if it's a valid move and false if not
    */
    static bool CheckMoveValidity(Stack stack, int ringValue)
    {
        if(stack.Count != 0) 
        {
            int topValue = stack.Peek();
        }
        //invalid move: adding larger ring on top of smaller ring
        if(ringValue > topValue)
        {
            return false;
        }
        //otherwise it's a valid move
        else
        {
            return true;
        }
    }
}
