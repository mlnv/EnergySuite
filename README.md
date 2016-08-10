# EnergySuite

[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](http://opensource.org/licenses/mit-license.php)

This is a simple real-time enegry based system for your Unity3d game. 

## Installation

* Apply last available [unitypackage](https://github.com/mlnv/EnergySuite/raw/master/EnergySuite/Builds/EnegrySuite.unitypackage) to your project
* Place EnergySuiteBehaviour prefab on your first scene (it's DontDestroyOnLoad script)
* Edit ***Password*** and ***PasswordSalt*** fields in _EnergySuiteConfig.cs_
* PROFIT

## Example Code

#### Don't forget to add this at the top of the script:

```csharp
using EnergySuite;
```

#### Get current energy amount:

```csharp
EnergySuiteManager.Amount;
```

#### Get max energy amount:

```csharp
EnergySuiteManager.MaxAmount;
```

#### Manually add amount of energy:

```csharp
EnergySuiteManager.Add(amount);
```

#### Manually use amount of energy:
(returns false if currentAmount < amountToUse)

```csharp
EnergySuiteManager.Use(amount);
```

#### Subscribe on energy amount changed action:

```csharp
EnergySuiteManager.OnEnergyChanged += OnEnergyAdded;

void OnEnergyAdded(int amount){
  CurrentAmountText.text = amount + "/" + EnergySuiteManager.MaxAmount;
}
```

#### Subscribe on time left tick action :
(updated every sec if not full)

```csharp
EnergySuiteManager.OnTimeLeftChanged += OnTimeLeftChanged;

void OnTimeLeftChanged(TimeSpan timeLeft){
  string formatString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
  TimeLeftText.text = formatString;
}
```

#### Convert time left value to slider value:

```csharp
TimeLeftSlider.value = EnergySuiteManager.ConvertToSliderValue(timeLeft);
```

All examples you can find at Example folder.

## TODO
- [x] Encrypted PlayerPrefs
- [ ] Native iOS/Android time check
- [ ] Simple handler solution for web server
- [ ] Native iOS/Android notification system

## Dependencies

This asset use [Secured PlayerPrefs](https://www.assetstore.unity3d.com/en/#!/content/32357) and [FSM](https://github.com/thefuntastic/Unity3d-Finite-State-Machine), so if you already have one of this asset in your project - just delete one copy of it.

Developed By
-------
Maksym Yemelianov

<a href="https://twitter.com/makmlnv">
<img alt="Follow me on Twitter"
src="http://i.imgur.com/Y6YCiG3.png" width="50"/>
</a>

License
-------
The MIT License (MIT)

Copyright (C) 2016 Maksym Yemelianov

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
