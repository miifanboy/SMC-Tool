<div id="top"></div>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/miifanboy/SMC-Tool">
    <img src="images/smctoollogo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">SMC Tool</h3>

  <p align="center">
    Simple Mod Creation Tool for Counter-Strike: Global Offensive
    <br />
    <a href="https://github.com/miifanboy/SMC-Tool"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/miifanboy/SMC-Tool">View Demo</a>
    ·
    <a href="https://github.com/miifanboy/SMC-Tool/issues">Report Bug</a>
    ·
    <a href="https://github.com/miifanboy/SMC-Tool/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
        <li><a href="#features">Features</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
        <li><a href="#building-from-source">Building From Source</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#to-do">To Do</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

This is my app to create simple mods for Counter-Strike: Global Offensive if you want to request any new features, you can comment on feature request issue after checking if its already on to-do list.

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

* [MaterialSkin](https://github.com/leocb/MaterialSkin)
* [Gameloop.Vdf](https://github.com/shravan2x/Gameloop.Vdf)
* [.Net Framework 4.7.2](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)
* [FodyCostura](https://github.com/Fody/Costura)
* [GithubUpdateCheck](https://github.com/Mayerch1/GithubUpdateCheck)
<p align="right">(<a href="#top">back to top</a>)</p>

### Features

-Built-in mod loader

-Option to export for [MIGI](https://github.com/ZooLSmith/MIGI3)

-[Vac Bypass](https://www.youtube.com/watch?v=QDia3e12czc)


<!-- GETTING STARTED -->
## Getting Started

Let's get started..

### Prerequisites
You need:

* .Net Framework 4.7.2

### Installation

1. Download the latest release.
2. Make sure that smctool.exe is in the correct directory containing data,addons,pak01 folders and some other files.
3. Execute smctool.exe with admin rights **(Admin rights are needed to create symbolic links between files)**
4. You have installed successfully.

<p align="right">(<a href="#top">back to top</a>)</p>

### Building from Source

1. Download the source code
2. Open the .sln file
3. Wait for the project to fully load.
4. Click build-> build smctool
5. Download data.zip from releases and extract the files to the same directory as smctool.exe

<!-- USAGE EXAMPLES -->
## Usage

It's usage is pretty straight forward:

### Simple Usage

For example if you want to make deagle full auto , you can select deagle and check "is full auto" checkbox then hit apply to save the selected weapon attributes in the app.Then hit File->Save to save all weapons to a file. After that, you can click "Load Mod" button to start csgo automatically with your mod.

<p align="right">(<a href="#top">back to top</a>)</p>

### In-Depth Usage

#### Apply Button:

-This button saves the current changes in the app

***Note that this doesn't save your changes to a file so if you close your app , the changes you have done will be lost. So make sure to click File->Save to save to a file**

#### Load Mod:

-This button reads "items_game_smcmod.txt" and creates all the neccessary files and launches csgo with your mod. You should use it after you have saved your mod via File->Save.

#### Load Defaults:

-This button loads the default variables for your current selected weapon.

#### Vac Bypass:

-It works %100. It is not a rickroll. I promise.

#### File->Open:

-Loads the selected file.

#### File->Save:

-Saves all weapons as items_game_smcmod.txt to the same directory as smctool.exe

#### File->Save As:

-Saves all weapons to a custom location with a custom name

***(If you are going to use the mod loader in this application use File->Save instead)**

#### File->Export For Migi

-Export your mod for [MIGI](https://github.com/ZooLSmith/MIGI3) (You need to save it first)

***(Make sure you have [MIGI](https://github.com/ZooLSmith/MIGI3) installed before using this option)**
<p align="right">(<a href="#top">back to top</a>)</p>



<!-- To Do -->
## To Do

- [ ] Creating scoped weapons
- [ ] Replacing models
- [ ] Custom Attributes

See the [open issues](https://github.com/miifanboy/SMC-Tool/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/epicfeature`)
3. Commit your Changes (`git commit -m 'Removed rickroll'`)
4. Push to the Branch (`git push origin feature/epicfeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Mii Fanboy - [@miifanboy](https://twitter.com/miifanboy) - Email: miifanboy@pm.me

Project Link: [https://github.com/miifanboy/SMC-Tool](https://github.com/miifanboy/SMC-Tool)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [shravan2x/Gameloop.Vdf](https://github.com/shravan2x/Gameloop.Vdf) - Vdf Parser Library
* [Best-README-Template](https://github.com/othneildrew/Best-README-Template) - Readme Template
* [Fody/Costura](https://github.com/Fody/Costura) - Library to include other assemblies with the executable
* [leocb/MaterialSkin](https://github.com/leocb/MaterialSkin) - Modern UI for C# Winforms
<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/miifanboy/SMC-Tool.svg?style=for-the-badge
[contributors-url]: https://github.com/miifanboy/SMC-Tool/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/miifanboy/SMC-Tool.svg?style=for-the-badge
[forks-url]: https://github.com/miifanboy/SMC-Tool/network/members
[stars-shield]: https://img.shields.io/github/stars/miifanboy/SMC-Tool.svg?style=for-the-badge
[stars-url]: https://github.com/miifanboy/SMC-Tool/stargazers
[issues-shield]: https://img.shields.io/github/issues/miifanboy/SMC-Tool.svg?style=for-the-badge
[issues-url]: https://github.com/miifanboy/SMC-Tool/issues
[license-shield]: https://img.shields.io/github/license/miifanboy/SMC-Tool.svg?style=for-the-badge
[license-url]: https://github.com/miifanboy/SMC-Tool/blob/master/LICENSE.txt
[product-screenshot]: images/screenshot.png
