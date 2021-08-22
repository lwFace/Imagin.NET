# Imagin .NET ![](https://img.shields.io/badge/style-5.1-blue.svg?style=flat&label=Version) ![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build) 
A framework for developing WPF apps with a modest emphasis on customization.

## Apps

There are seven experimental apps that demonstrate what the library is capable of.

### *Alarm* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Set an alarm that repeats every so often until disabled.

**Features**
* Sets a repeating alarm using an interval you specify.
* Plays a .mp3 or .wav file you specify.
* Can require a math problem be solved before disabling (or snoozing) the alarm.
* Can force system volume at a certain level so the alarm is guarenteed to be heard.

**Why?**
> I am very displeased with the default app on Windows 10 (it is slow!). I also like setting multiple alarms...this combines (and extends) those features so multiple alarms can be set with just two clicks.

### *Color* ![](https://img.shields.io/badge/style-Stable--ish-yellow.svg?style=flat&label=Build)

Find, convert, and save multiple colors.

**Features**
* Similar to the color picker in *Photoshop*.
* Introduces several color models nonexistant in most software to visualize colors even further.
* Enables converting to and from each color model, including hexadecimal and models that can't be visualized like *CMYK* and *XYZ*.
* Utilizes *DirectX* to maximize performance.

**Why?**
> I wanted an app designed just for picking colors. It is marginally more lightweight than **Paint** and has similar (if not better) performance than **Photoshop**.

### *Desktop* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Draw tiles on the desktop to display notes, count downs, clocks, images, slideshows, and entire folders.

**Features**
* Tiles may include a single photo, a slideshow of photos, or a folder of files and folders.
* Tiles can be resized, moved, and the background color can be customized.

**Why?**
> The only apps I know of that offer these features are locked behind a pay wall. My solution does not tap much into Windows programming (a sad pitfall), but still adopts a similarly effective approach.

### *Explorer* ![](https://img.shields.io/badge/style-Stable--ish-yellow.svg?style=flat&label=Build)

Explore file system visually or by command line.

**Features**
* Includes traditional *Windows* context menus.
* Perform mass rename operations on file names and extensions.

**Why?**
> I wanted something that I could customize on my own terms instead of digging into lower-level Windows stuff.

### *Notes* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Manage multiple text and list files.

**Features**
* Manage a folder of text (.txt) and list (.list) files called "notes".
* List items may specify "attributes", which can add useful information, such as prices or dates (think of shopping lists and count downs).
* Lists can be sorted and are stored as XML.
* Notes are automatically sorted by the latest changes.
* Includes all features of Notepad.

**Why?**
> I like seeing all my notes in one place. I also like making lists. This ultimately eliminates the need to use an app like **Microsoft Word** or **Excel** to make simple notes and lists.

### *Paint* ![](https://img.shields.io/badge/style-Unstable-red.svg?style=flat&label=Build)

Create, open, and save image files.

**Features**
* A raster graphics editor I designed for fun.
* There is a focus on filters, similar to *Instagram*.
* Utilizes *DirectX*, but leaves much to be desired.

**Why?**
> I wanted to see if I could imitiate what **Paint** and **Adobe Photoshop** do. So far, I have realized much of this relies on very specific algorithms. As I am not enthusastic about reinventing the wheel (a really big wheel), I have left much unfinished. This is a for-fun project for anyone who wants to take it a step further. Fortunately, many of the concepts are already there!

### *Vault* ![](https://img.shields.io/badge/style-Stable--ish-yellow.svg?style=flat&label=Build)

Sync folders and manage passwords.

**Features**
* Copies, encrypts, or decrypts a folder at one location (the source) to another location (the destination). 
* Tasks can include and exclude both files and folders.
* Can encrypt and decrypt file and folder names as well as file content.
* When a task is enabled, changes are observed in real-time.
* All task activity can be logged.
* Manage (and optionally encrypt) passwords.
* Generate random strings of characters.
* A utility for quickly encrypting and decrypting text.

**Why?**
> I wanted an app that could easily back up any folder on my computer in real-time. Encryption and decryption was a must because I prefer to encrypt my own data before backing it up to the cloud. Encryption and decryption in this app is experimental and based on algorithms I found online. Therefore, I can't guarentee this app can reliably (and accurately) encrypt or decrypt your data. It is ultimately up to you (until noted otherwise) to verify the reliability and accuracy of the encryption and decryption!

#### Build status

  Status  |  Description  |
--------|------------|
![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build) | Nothing currently known can cause app to crash; most or all features are complete.
![](https://img.shields.io/badge/style-Stable--ish-yellow.svg?style=flat&label=Build) | Some features can cause app to crash and/or insufficient testing was performed to reach a meaningful conclusion; some features are incomplete or do not exist.
![](https://img.shields.io/badge/style-Unstable-red.svg?style=flat&label=Build) | Most features can cause app to crash; many features are incomplete or do not exist.

## Projects

### *Imagin.Common* ![](https://img.shields.io/badge/style-C%23-blue.svg?style=flat&label=Language)

Defines common utilities for use in a shared environment.

**Dependencies**

  Name  |  Version  |  Url  |
--------|-----------|-------|
NETStandard.Library | 2.0.3 | 
InheritDoc | 2.5.2 | https://www.inheritdoc.io/

### *Imagin.Common.WPF* ![](https://img.shields.io/badge/style-C%23-blue.svg?style=flat&label=Language)

Defines common user interface elements for use in WPF apps.

**Dependencies**

  Name  |  Version  |  Url  |
--------|-----------|-------|
GongSolutions.WPF.DragDrop | 1.1 | https://github.com/punker76/gong-wpf-dragdrop |
Imagin.Common |  |  |
Magick.NET-Q16-AnyCPU | 7.0.4.100 | https://github.com/dlemstra/Magick.NET |
System.Reflection.TypeExtensions | 4.4.0 |  |
System.Windows.Interactivity.WPF | 2.0.20525 | http://www.microsoft.com/en-us/download/details.aspx?id=10801 |
WriteableBitmapEx.Wpf | 1.5 | https://github.com/reneschulte/WriteableBitmapEx |
XAMLMarkupExtensions | 1.3.0 | http://xamlmarkupextensions.codeplex.com/ |

### *Demo* ![](https://img.shields.io/badge/style-C%23-blue.svg?style=flat&label=Language)

Showcases all controls and their properties. This is primarily used for *testing*.

### *MasterVolume* ![](https://img.shields.io/badge/style-C++-red.svg?style=flat&label=Language)

Allows a program to increase or decrease the system volume.

  Name  |  Version  |
--------|-----------|
mscorlib | 4.0 |

## Dependencies

All third-party dependencies are located in the sub-folder **Libary**.

## Documentation

Documentation is sparse and that is both intentional and unintentional. I am picky with wording things and a lot of it would be redundant anyway so I try to avoid it unless something super complex requires further clarification. For the most part, the framework is designed in a manner that is easy to understand, but not everything is obvious to everyone so I do understand the importance of documentation. I plan on including more in the future, but I will still strive to keep it simple.

### [InheritDoc](https://www.inheritdoc.io/)
Referenced by *Imagin.Common* and **cannot** be used anywhere else.

## Nuget
Refer here for latest changes!

> **5/27/2021**: All packages obsolete. DO NOT USE.
    
## Donate

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AJJG6PWLBYQNG)
