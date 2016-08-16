# EnergySuite

This is a simple real-time enegry based system for your Unity3d game. 

<img alt="Follow me on Twitter"
src="http://i.imgur.com/fT7ZJTB.png" width="250"/>

## Installation

* Apply last available [unitypackage](https://github.com/mlnv/EnergySuite/raw/master/EnergySuite/Builds/EnegrySuite.unitypackage) to your project
* Place EnergySuiteBehaviour prefab on your first scene (it's DontDestroyOnLoad script)
* Edit ***Password*** and ***PasswordSalt*** fields in _EnergySuiteConfig.cs_
* Add your time values to **TimeValue** enum
* Place you time values to **StoredInfo** field in _EnergySuiteConfig.cs_
* PROFIT

## Example Code

#### Don't forget to add this at the top of the script:

```csharp
using EnergySuite;
```

#### Get current time value amount:

```csharp
EnergySuiteManager.GetAmount(timeValueType)
```

#### Get max time value amount:

```csharp
EnergySuiteManager.GetMaxAmount(timeValueType)
```

#### Manually add amount of time value:

```csharp
EnergySuiteManager.Add(timeValueType, amount);
```

#### Manually use amount of time value:
(returns false if currentAmount < amountToUse)

```csharp
EnergySuiteManager.Use(timeValueType, amount);
```

#### Subscribe on time value amount changed action:

```csharp
EnergySuiteManager.OnAmountChanged += OnAmountChanged;

void OnAmountChanged(int amount, TimeBasedValue timeBasedValue) {

  string text = amount + "/" + timeBasedValue.MaxAmount;
  
  switch (timeBasedValue.Type) {
	  case TimeValue.Life:
		  CurrentLifeAmountText.text = text;
			break;
		case TimeValue.Key:
		  CurrentKeyAmountText.text = text;
			break;
		default:
			break;
	}
}
```

#### Subscribe on time left tick action :
(updated every sec if not full)

```csharp
EnergySuiteManager.OnTimeLeftChanged += OnTimeLeftChanged;

void OnTimeLeftChanged(TimeSpan timeLeft, TimeBasedValue timeBasedValue) {

  string formatString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
	float sliderValue = EnergySuiteManager.ConvertToSliderValue(timeLeft, timeBasedValue);
  
  switch (timeBasedValue.Type) {
	  case TimeValue.Life:
		  LifeTimeLeftText.text = formatString;
			LifeTimeLeftSlider.value = sliderValue;
		  break;
		case TimeValue.Key:
		  KeyTimeLeftText.text = formatString;
			KeyTimeLeftSlider.value = sliderValue;
			break;
		default:
			break;
	}
}
```

#### Convert time left value to slider value:

```csharp
TimeLeftSlider.value = EnergySuiteManager.ConvertToSliderValue(timeLeft, timeBasedValue);
```

All examples you can find at Example folder.

## TODO
- [x] Encrypted PlayerPrefs
- [x] Native iOS/Android time check
- [x] Make system handle many simultaneous timers
- [ ] Simple handler solution for web server
- [ ] Native iOS/Android notification system

## Dependencies

This asset use [Secured PlayerPrefs](https://www.assetstore.unity3d.com/en/#!/content/32357) and [StateKit](https://github.com/prime31/StateKit), so if you already have one of this asset in your project - just delete one copy of it.

Developed By
-------
Maksym Yemelianov

<a href="https://twitter.com/makmlnv">
<img alt="Follow me on Twitter"
src="http://i.imgur.com/Y6YCiG3.png" width="50"/>
</a>

License
-------
[Attribution-NonCommercial-ShareAlike 3.0 Unported](http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode) with [simple explanation](http://creativecommons.org/licenses/by-nc-sa/3.0/deed.en_US) with the attribution clause waived. You are free to use StateKit in any and all games that you make. You cannot sell StateKit directly or as part of a larger game asset.
