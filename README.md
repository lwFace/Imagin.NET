# Imagin .NET ![](https://img.shields.io/badge/style-5.4-blue.svg?style=flat&label=Version) ![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build) 
A framework for developing WPF apps.

## Apps

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Alarm.png?raw=true" width="64" /> *Alarm* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

An alarm that repeats every so often.

**Features**
* Sets a repeating alarm using an interval you specify.
* Plays a .mp3 or .wav file you specify.
* Can require a math problem be solved before disabling (or snoozing) the alarm.
* Can force system volume at a certain level so the alarm is guarenteed to be heard.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Color.png?raw=true" width="64" /> *Color* ![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build)

Save and convert colors.

**Features**
* A lightweight version of the color picker in *Photoshop*.
* Introduces several color models nonexistant in most software to visualize colors even further.
* Enables converting to and from each color model, including hexadecimal and models that can't be visualized like *CMYK* and *XYZ*.
* Utilizes *DirectX* to maximize performance.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Desktop.png?raw=true" width="64" /> *Desktop* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Draw tiles on the desktop to display notes, count downs, clocks, images, slideshows, and folders.

**Features**
* Tiles may include a single photo, a slideshow of photos, or a folder of files and folders.
* Tiles can be resized, moved, and the background color can be customized.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Explorer.png?raw=true" width="64" /> *Explorer* ![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build)

Explore file system.

**Features**
* Includes traditional *Windows* context menus.
* Execute commands using a command-line interface.
* Minimal lower-level, Windows stuff.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Notes.png?raw=true" width="64" /> *Notes* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Make notes and lists.

**Features**
* Manage a folder of text (.txt) and list (.list) files called "notes".
* List items may specify "attributes", which can add useful information, such as prices or dates (think of shopping lists and count downs).
* Lists can be sorted and are stored as XML.
* Notes are automatically sorted by the latest changes.
* Includes all features of Notepad.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Paint.png?raw=true" width="64" /> *Paint* ![](https://img.shields.io/badge/style-Unstable-red.svg?style=flat&label=Build)

Create and save images.

**Features**
* A raster graphics editor I designed for fun.
* There is a focus on filters, similar to *Instagram*.
* Utilizes *DirectX*, but leaves much to be desired.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Random.png?raw=true" width="64" /> *Random* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Generate random strings of any length and character.

**Features**
* Generate random strings (asynchronously). 
* Specify a length.
* Type the characters to include.
* Add characters from presets.
* Custom presets for adding characters.
* Customize appearance of generated strings.

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Rename.png?raw=true" width="64" /> *Rename* ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build)

Rename multiple file names and extensions at once.

**Features**
* Rename multiple file names and extensions.
* Include sub directories or just top directory.
* Target files with certain extensions.
* Target files with certain names.
* Correct file extension casing

### <img align="left" src="https://github.com/imagin-tech/Imagin.NET/blob/master/Logos/Vault.png?raw=true" width="64" /> *Vault* ![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build)

Sync folders and manage passwords.

**Features**
* Create tasks to copy, encrypt, or decrypt a folder at one location (the source) to another location (the destination). 
* When a task is enabled, changes are observed in real-time.
* Tasks can include and exclude both files and folders.
* Encrypt and decrypt file and folder names as well as file content.
* Log all task activity.
* Manage (and optionally encrypt) passwords.
* Quickly encrypt and decrypt arbitrary text.

#### Build status

  Status  |  Description  |  Features  |
----------|---------------|------------|
![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=Build) | Nothing currently known can cause app to crash | Most or all complete |
![](https://img.shields.io/badge/style-Experimental-yellow.svg?style=flat&label=Build) | Insufficient testing performed to reach meaningful conclusion | Some incomplete or do not exist |
![](https://img.shields.io/badge/style-Unstable-red.svg?style=flat&label=Build) | Some features can cause app to crash | Most incomplete or do not exist |

## Projects

### *Imagin.Common* ![](https://img.shields.io/badge/style-C%23-blue.svg?style=flat&label=Language)

Common utilities for shared projects.

**Dependencies**

  Name  |  Version  |  Url  |
--------|-----------|-------|
NETStandard.Library | 2.0.3 | 
InheritDoc | 2.5.2 | https://www.inheritdoc.io/

### *Imagin.Common.WPF* ![](https://img.shields.io/badge/style-C%23-blue.svg?style=flat&label=Language)

Common user interface elements.

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

Showcases all controls. This is for *testing*.

### *MasterVolume* ![](https://img.shields.io/badge/style-C++-red.svg?style=flat&label=Language)

Allows a program to increase or decrease the system volume.

  Name  |  Version  |
--------|-----------|
mscorlib | 4.0 |

## Dependencies

Third-party dependencies are located in the **Libary** folder.

  Name  |  Version  |
--------|-----------|
GongSolutions.WPF.DragDrop | 1.1 |
Hardcodet.Wpf.TaskbarNotification | 1.0.5 |
Magick.NET-Q16-AnyCPU | 7.0.4.100 |
System.Windows.Interactivity | 2.0.20525 |
WriteableBitmapEx.Wpf | 1.5 |
XAMLMarkupExtensions | 1.2.2 |

## Languages

  Language  |
------------|
English |
Spanish |
Italian |
Japanese |

## Themes

  Theme  |
---------|
Blaze |
Chocolate |
Dark |
Jungle |
Light |
Midnight |
Violet |

## Nuget
All packages deprecated (8/29/2021).
    
## Donate

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AJJG6PWLBYQNG)
