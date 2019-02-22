# WoxDictionary
![Demonstration](Images/demo.gif)

这是一个支持中英词语翻译，自动纠正，近义词查找的词典 [Wox](https://github.com/Wox-launcher/Wox) 插件。其大部分功能都支持离线使用。

This is a [Wox](https://github.com/Wox-launcher/Wox) plugin that supports English/Chinese word translation, word correction and synonym. Most of its functions can work offline.

## 功能 / Features

* 中英互译 English/Chinese word translation

  ![cn_to_en](Images/demo/cn_to_en.png)

  ![en_to_cn](Images/demo/en_to_cn.png)

* 音标 Phonetic

  ![Phonetic](Images/demo/phonetic.png)

* 中文释义 Translation

  单词结尾加!t即可触发. Add !t at the end of the word to trigger.

  ![Translation](Images/demo/translation.png)

* 英文定义 Definition

  单词结尾加!d即可触发. Add !d at the end of the word to trigger.

  ![Definition](Images/demo/definition.png)

* 近义词 Synonym

  单词结尾加!s即可触发. Add !s at the end of the word to trigger.

  ![Synonym](Images/demo/synonym.png)

* 变型 Exchanges

  单词结尾加!e即可触发. Add !e at the end of the word to trigger.

  ![Exchanges](Images/demo/exchanges.png)

* 拼写修正 Spelling correction

  ![Spelling correction](Images/demo/spelling_correction.png)

* 快捷复制 Word copy

  按 Alt+Enter 即可复制单词 Pressing Alt+Enter can copy the word.

## 编译 / Compilation

1. 使用 Visual Studio 编译此项目。 Compile it using Visual Studio.
2. 运行`generate_dist.bat` 以自动打包。 Pack it using `generate_dist.bat`

## 安装 / Installation

### 

1. 你需要先安装 [Wox](https://github.com/Wox-launcher/Wox)。 

   You need to have [Wox](https://github.com/Wox-launcher/Wox) installed first.

2. (手动安装) 复制 dist 文件夹到 `C:\Users\\{User Name}\AppData\Local\Wox\app-{Version Number}\Plugins\`

   (Manual Installation) Copy dist folder to `C:\Users\\{User Name}\AppData\Local\Wox\app-{Version Number}\Plugins\`

   (WPM 安装) 在 Wox 中输入 `wpm install Dictionary`。

   (Wox Plugin Manager) Type `wpm install Dictionary` in your Wox.

3. **下载，解压，复制 [ecdict.db](https://github.com/harrynull/WoxDictionary/releases/tag/dict) 到 `C:\Users\用户名\AppData\Roaming\Wox\Plugins\Dictionary-随机串\dicts`。**

   **Download, uncompress and copy [ecdict.db](https://github.com/harrynull/WoxDictionary/releases/tag/dict) to `C:\Users\{your_user_name}\AppData\Roaming\Wox\Plugins\Dictionary-{random_characters}\dicts`.**

4. 打开设置，配置 API tokens.

   Open settings and configure API tokens.


## 致谢 / Acknowledgment

这个项目没有以下伟大项目的帮助是不可能完成的：

The project won't be possible without the help of the following great projects:

* [Wox](https://github.com/Wox-launcher/Wox) Launcher for Windows, an alternative to Alfred and Launchy.
* [ECDICT](https://github.com/skywind3000/ECDICT) Free English to Chinese Dictionary Database. By Linwei.
* [SymSpell](https://github.com/wolfgarbe/SymSpell) 1 million times faster through Symmetric Delete spelling correction algorithm. By Wolf Garbe.
* [Big Huge Thesaurus](https://words.bighugelabs.com/api.php) A very simple API for retrieving the synonyms for any word.
* [iciba](http://open.iciba.com/?c=api) Chinese traslation API.

## License

This project is released under LGPL 3.0 License.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
