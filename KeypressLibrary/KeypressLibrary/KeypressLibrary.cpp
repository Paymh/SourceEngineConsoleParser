#include "stdafx.h"

extern "C" __declspec(dllexport) void Presskey(WORD key);
extern "C" __declspec(dllexport) void Releasekey(WORD key);
extern "C" __declspec(dllexport) void Movemouse(int amountX, int amountY);
extern "C" __declspec(dllexport) void wait(int ms);
void Presskey(WORD key)
{
	INPUT ip = {0};
	ip.type = INPUT_KEYBOARD;
	ip.ki.wScan = MapVirtualKey(key,MAPVK_VK_TO_VSC);
	ip.ki.time = 0;
	ip.ki.dwExtraInfo = GetMessageExtraInfo();
	ip.ki.wVk = key; 
	ip.ki.dwFlags = 0;
	SendInput(1, &ip, sizeof(INPUT));
}

void Releasekey(WORD key)
{
	INPUT ip = {0};
	ip.type = INPUT_KEYBOARD;
	ip.ki.wScan = MapVirtualKey(key,MAPVK_VK_TO_VSC);
	ip.ki.time = 0;
	ip.ki.dwExtraInfo = GetMessageExtraInfo();
	ip.ki.wVk = key; 
	ip.ki.dwFlags = 2;
	SendInput(1, &ip, sizeof(INPUT));
}

void Movemouse(int amountX = 0, int amountY = 0)
{
	POINT mousePos;
	GetCursorPos(&mousePos);
	SetCursorPos(mousePos.x + amountX,mousePos.y + amountY);
}

void wait(int ms) {
	long ticks = GetTickCount64();
	long ticksNeeded = ticks + ms;

	while (GetTickCount64() < ticksNeeded);
}
