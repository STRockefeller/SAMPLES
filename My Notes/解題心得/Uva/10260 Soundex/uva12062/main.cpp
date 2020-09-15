#include <iostream>

/*
1 represents B, F, P, or V
2 represents C, G, J, K, Q, S, X, or Z
3 represents D or T
4 represents L
5 represents M or N
6 represents R
*/
using namespace std;

int main ()
{
    char inputStr[20];
    cin>>inputStr;

    int print,prePrint;
    for(int i=0;i<20;i++)
    {
        switch(inputStr[i])
        {
            case 'B':
            case 'F':
            case 'P':
            case 'V':
                print=1;
                //cout<<"1";
                break;
            case 'C':
            case 'G':
            case 'J':
            case 'K':
            case 'Q':
            case 'S':
            case 'X':
            case 'Z':
                print=2;
                //cout<<"2";
                break;
            case 'D':
            case 'T':
                print=3;
                //cout<<"3";
                break;
            case 'L':
                print=4;
                //cout<<"4";
                break;
            case 'M':
            case 'N':
                print=5;
                //cout<<"5";
                break;
            case 'R':
                print=6;
                //cout<<"6";
                break;
            default:
                break;
        }
        if(print!=prePrint)
            cout<<print;
        prePrint=print;
    }
    return 0;
}
