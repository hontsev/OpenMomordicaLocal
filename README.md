# OpenMomordicaLocal 开源苦瓜单机版

Momordica VDM - local bot version

虚拟人形苦瓜 - 单机版

## 基本介绍

- 本程序是一个PC桌面版单机聊天bot，提供多种聊天自动回复功能

## 具备功能

- 语料随机拼接回复
- roll dice 掷骰子
- 淫梦民最喜欢的全自动数字论证
- ~~百度知识图谱信息检索~~（受百度反爬虫限制，不太好用）
- 百度百科/百度贴吧内容检索
- 赛🐎小游戏
- 图片转字符画
- 特殊符号文本生成
- ~~藏头诗、藏尾诗生成~~（语料库比较大，暂未同步该模块所需数据）
- 多语种翻译（基于google translate）
- 抽卡
- 周易占卜（赛博蓍草法起卦）
- ……


## 开发须知

- 程序运行依赖库 `.net framework 4.5+` 
- 本程序的酷Q（CoolQ）版本程序敬请参考[此库](https://github.com/hontsev/OpenMomordica) ，部分功能有差别。
- 运行时资源及配置文件放置在 `运行路径/RunningData` 里。具体文件类型和数据格式暂无说明，请参考代码中的读取过程。程序运行路径下 `config.txt` 可进行运行时参数配置。
- 建议使用Visual Studio 2015 或更高版本进行开发（需要支持C# 6.0语法糖），否则可能会编译报错。




