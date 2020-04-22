//必須#include <vector> using namespace std;

#include <iostream>
#include <vector>

using namespace std;

int main()
{
	vector<string> vStr;
	//加入項目
	vStr.emplace_back("LaDiDa");
	vStr.emplace_back("Rockefeller");
	vStr.emplace_back("DamDaDiDoo");

	//刪除最後一項目
	//vStr.pop_back();

	//全刪
	//vStr.~vector();
	//vStr.clear();

	//C++版 foreach
	for (auto& vstr : vStr)
	{
		cout << vstr << endl;
	}

	auto slice = vector<string>(vStr.begin(), vStr.begin() + 1);

	//size() 項目數 max_size() 記憶體占用
	cout << "Size:" << vStr.size() << endl << "Max Size:" << vStr.max_size() << endl;

	//是否為空 回傳bool
	cout << vStr.empty() << endl;
	return 0;
}