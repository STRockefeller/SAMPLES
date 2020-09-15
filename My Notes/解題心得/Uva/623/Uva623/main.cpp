#include <iostream>

using namespace std;


class Factorial
{
public:
    void printNum(long num)
    {
        if(factorialNum[num]!=0)
            cout<<num<<"!"<<endl<<factorialNum[num]<<endl;
        else
        {
            factorialNum[num] = factorialize(num);
            cout<<num<<"!"<<endl<<factorialNum[num]<<endl;
        }
    }
private:
    long factorialNum[500] = {0};
    long factorialize(long num)
    {
        switch(num)
        {
        case 0:
            return 0;
        case 1:
            return 1;
        default:
            return num*factorialize(num-1);
        }
    }
};

int main()
{
    long inputNum;
    Factorial fac;
    while(1)
    {
        cin>>inputNum;
        fac.printNum(inputNum);
    }




    return 0;
}
