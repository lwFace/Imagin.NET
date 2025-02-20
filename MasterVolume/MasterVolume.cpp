// MasterVolume.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>
#include <mmdeviceapi.h>
#include <endpointvolume.h>

#define DLLExport __declspec(dllexport)

int Main()
{
	return 0;
}

extern "C"
{
	enum class VolumeUnit
	{
		Decibel,
		Scalar
	};

	//Gets volume
	DLLExport float GetVolume(VolumeUnit vUnit)
	{
		HRESULT hr;

		CoInitialize(NULL);
		IMMDeviceEnumerator *deviceEnumerator = NULL;
		hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
		IMMDevice *defaultDevice = NULL;

		hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
		deviceEnumerator->Release();
		deviceEnumerator = NULL;

		IAudioEndpointVolume *endpointVolume = NULL;
		hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
		defaultDevice->Release();
		defaultDevice = NULL;

		float currentVolume = 0;
		if (vUnit == VolumeUnit::Decibel)
		{
			hr = endpointVolume->GetMasterVolumeLevel(&currentVolume);
		}
		else if (vUnit == VolumeUnit::Scalar)
		{
			hr = endpointVolume->GetMasterVolumeLevelScalar(&currentVolume);
		}
		endpointVolume->Release();
		CoUninitialize();

		return currentVolume;
	}

	//Sets volume
	DLLExport void SetVolume(double newVolume, VolumeUnit vUnit)
	{
		HRESULT hr;

		CoInitialize(NULL);
		IMMDeviceEnumerator *deviceEnumerator = NULL;
		hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
		IMMDevice *defaultDevice = NULL;

		hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
		deviceEnumerator->Release();
		deviceEnumerator = NULL;

		IAudioEndpointVolume *endpointVolume = NULL;
		hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
		defaultDevice->Release();
		defaultDevice = NULL;

		if (vUnit == VolumeUnit::Decibel)
		{
			hr = endpointVolume->SetMasterVolumeLevel((float)newVolume, NULL);
		}
		else if (vUnit == VolumeUnit::Scalar)
		{
			hr = endpointVolume->SetMasterVolumeLevelScalar((float)newVolume, NULL);
		}
		endpointVolume->Release();
		CoUninitialize();
	}

	DLLExport bool Muted()
	{
		HRESULT hr;

		CoInitialize(NULL);
		IMMDeviceEnumerator *deviceEnumerator = NULL;
		hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
		IMMDevice *defaultDevice = NULL;

		hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
		deviceEnumerator->Release();
		deviceEnumerator = NULL;

		IAudioEndpointVolume *endpointVolume = NULL;
		hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
		defaultDevice->Release();
		defaultDevice = NULL;

		BOOL *result = false;
		hr = endpointVolume->GetMute((BOOL *)&result);
		endpointVolume->Release();

		CoUninitialize();
		return result;
	}

	void mute(bool input)
	{
		HRESULT hr;

		CoInitialize(NULL);
		IMMDeviceEnumerator *deviceEnumerator = NULL;
		hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator), (LPVOID *)&deviceEnumerator);
		IMMDevice *defaultDevice = NULL;

		hr = deviceEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &defaultDevice);
		deviceEnumerator->Release();
		deviceEnumerator = NULL;

		IAudioEndpointVolume *endpointVolume = NULL;
		hr = defaultDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_INPROC_SERVER, NULL, (LPVOID *)&endpointVolume);
		defaultDevice->Release();
		defaultDevice = NULL;

		hr = endpointVolume->SetMute(input, NULL);
		endpointVolume->Release();
		CoUninitialize();
	}

	DLLExport void Mute()
	{
		mute(true);
	}

	DLLExport void Unmute()
	{
		mute(false);
	}
}