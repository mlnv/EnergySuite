# EnergySuite

This is a simple real-time enegry based system for your Unity3d game. 

## Installation

* Apply last available [unitypackage](https://github.com/mlnv/EnergySuite/raw/master/EnergySuite/Builds/EnegrySuite.unitypackage) to your project
* Place EnergySuiteBehaviour prefab on your first scene (it's DontDestroyOnLoad script)
* PROFIT

## Example Code

Don't forget to add this at the top of the script:

```
using EnergySuite;
```

Get current energy amount:

```
EnergySuiteManager.Amount;
```

Get max energy amount:

```
EnergySuiteManager.MaxAmount;
```

Manually add amount of energy:

```
EnergySuiteManager.Add(amount);
```

Manually use amount of energy (returns false if currentAmount < amountToUse):

```
EnergySuiteManager.Use(amount);
```

Subscribe on energy amount changed action:

```
EnergySuiteManager.OnEnergyChanged += OnEnergyAdded;

void OnEnergyAdded(int amount){
  CurrentAmountText.text = amount + "/" + EnergySuiteManager.MaxAmount;
}
```

Subscribe on time left tick action (updated every sec if not full):

```
EnergySuiteManager.OnTimeLeftChanged += OnTimeLeftChanged;

void OnTimeLeftChanged(TimeSpan timeLeft){
  string formatString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
  TimeLeftText.text = formatString;
}
```

Convert time left value to slider value (0-1 float):

```
TimeLeftSlider.value = EnergySuiteManager.ConvertToSliderValue(timeLeft);
```

All examples you can find at Example folder.

## TODO
- [x] Encrypted PlayerPrefs
- [ ] Native iOS/Android time check
- [ ] Simple handler solution for web server

License
-------

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
