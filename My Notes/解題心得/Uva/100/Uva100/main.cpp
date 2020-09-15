#include <iostream>
/*
Sample Input
1 10
100 200
201 210
900 1000
Sample Output
1 10 20
100 200 125
201 210 89
900 1000 174
*/
using namespace std;
int maxCycleLength(int n)
{
    int times=1;
    while(1)
    {
        if(n==1)
            break;
        else if(n%2==0)
            n/=2;
        else
            n=n*3+1;
        times++;
    }
    return times;
}
int main()
{
    int maxNum,minNum;
    int length=0;
    cin>>minNum;
    cin>>maxNum;
    for(int i=minNum;i<=maxNum;i++)
    {
        if(maxCycleLength(i)>length)
            length=maxCycleLength(i);
    }

    cout<<minNum<<" "<<maxNum<<" "<<length;
    return 0;
}
