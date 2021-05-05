# Hough変換

## 直線を検出する

```C++
#include <opencv2/opencv.hpp>
 
using namespace std;
 
int main(void)
{
	// 画像を格納するオブジェクトを宣言する
	cv::Mat	src1, src2, src_bin;
 
	// 画像ファイルから画像データを読み込む
	src1 = cv::imread("C:/opencv/sources/samples/data/stuff.jpg", cv::IMREAD_COLOR);
 
	if (src1.empty() == true) {
		// 画像データが読み込めなかったときは終了する
		return 0;
	}
	src2 = src1.clone();
 
	// エッジを検出
	cv::cvtColor(src1, src_bin, cv::COLOR_BGR2GRAY);
	cv::Canny(src_bin, src_bin, 50, 100);
 
	cv::imshow("Canny", src_bin);
 
	// 標準Hough変換
	vector<cv::Vec2f> lines;
 
	cv::HoughLines(src_bin, lines, 1, CV_PI / 180, 60);
 
	for (int i = 0; i < lines.size(); i++) {
		float  roh = lines[i][0], theta = lines[i][1];
		double x0 = roh * cos(theta), y0 = roh * sin(theta);
		double a = 1000;
 
		cv::Point p1(x0 - sin(theta) * a, y0 + cos(theta) * a);
		cv::Point p2(x0 + sin(theta) * a, y0 - cos(theta) * a);
 
		cv::line(src1, p1, p2, cv::Scalar(0, 255, 0), 1, cv::LINE_AA);
	}
	cv::imshow("標準Hough変換", src1);
 
	// プログレッシブな確率的Hough変換
	vector<cv::Vec4i> lines2;
 
	HoughLinesP(src_bin, lines2, 1, CV_PI / 180, 50, 10, 50);
 
	for (size_t i = 0; i < lines2.size(); i++) {
		cv::Vec4i p = lines2[i];
		cv::line(src2, cv::Point(p[0], p[1]), cv::Point(p[2], p[3]), cv::Scalar(0, 255, 0), 1, cv::LINE_AA);
	}
	cv::imshow("プログレッシブな確率的Hough変換", src2);
	
	// 何かキーが押されるまで待つ
	cv::waitKey();
 
	return 0;
}
```

## 円を検出する

```C++
#include <opencv2/opencv.hpp>
 
using namespace std;
 
int main(void)
{
	// 画像を格納するオブジェクトを宣言する
	cv::Mat	src, src_gray;
 
	// 画像ファイルから画像データを読み込む
	src = cv::imread("C:/opencv/sources/samples/data/stuff.jpg", cv::IMREAD_COLOR);
 
	if (src.empty() == true) {
		// 画像データが読み込めなかったときは終了する
		return 0;
	}
	cv::cvtColor(src, src_gray, cv::COLOR_BGR2GRAY);
 
	// Hough円変換
	vector<cv::Vec3f> circles;
 
	cv::HoughCircles(src_gray, circles, cv::HOUGH_GRADIENT, 1, 50, 100, 20, 1, 100);
 
	for (int i = 0; i < circles.size(); i++) {
		circle(src, cv::Point(circles[i][0], circles[i][1]), circles[i][2], cv::Scalar(0, 255, 0), 1, cv::LINE_AA);
	}
	cv::imshow("Hough円変換", src);
 
	// 何かキーが押されるまで待つ
	cv::waitKey();
 
	return 0;
}
```

