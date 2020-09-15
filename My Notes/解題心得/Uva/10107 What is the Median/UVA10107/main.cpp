#include <iostream>

using namespace std;

void exchange(int &a,int &b)
{
    int t=a;
    a=b;
    b=t;
}

int findMedium(int countNum,int intArr[20])
{
    int mediumNum=0;
    int newArr[countNum] = {0};
    for(int i=0;i<countNum;i++)
    {
        newArr[i] = intArr[i];
    }
    //region quickSort
    int pivot = 0;
    int leftSide,rightSide;
    Sort:
    leftSide = -1;
    rightSide = -1;
    for(int i=0;i<countNum;i++)
    {
        int j = countNum -1-i;
        if(newArr[i]>newArr[pivot] && i<pivot)
        {
            leftSide = i;
            exchange(newArr[i],newArr[pivot]);
        }
        if(newArr[j]<newArr[pivot] && j>pivot)
        {
            rightSide = j;
            exchange(newArr[j],newArr[pivot]);
        }

        if( i >= j && leftSide == -1 && rightSide == -1)
        {
            if((i>pivot && newArr[pivot]>newArr[i])||(i<pivot && newArr[i]>newArr[pivot]))
                exchange(newArr[pivot],newArr[i]);
            if(pivot < countNum-1)
                {
                    pivot++;
                    goto Sort;
                }
            break;
        }

    }
    //end region
    //region find medium
    switch(countNum%2)
    {
    case 1:
        mediumNum = newArr[countNum/2];
        break;
    case 0:
        mediumNum = (newArr[countNum/2]+newArr[countNum/2-1])/2;
        break;
    default:
        mediumNum = -1;
        break;
    }
    return mediumNum;

    //end region
}
int main()
{
    int intArr[20]={0};

    int countNum=1;
    while(1)
    {
        cout<<"input:";
        cin>>intArr[countNum-1];
        cout<<"Medium Number is:"<<findMedium(countNum,intArr)<<endl;

        countNum++;

    }
    return 0;
}
