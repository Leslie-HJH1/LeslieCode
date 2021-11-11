README.txt

# 【ユニティちゃんトゥーンシェーダー Ver.2.0.8】
「ユニティちゃんトゥーンシェーダー」は、セル風3DCGアニメーションの制作現場での要望に応えるような形で設計された、映像志向のトゥーンシェーダーです。  

    #【尤妮蒂酱toonshader Ver.2.0.8】
    “尤妮蒂卡通着色器”是为了满足赛璐璐风格3d cg动画制作现场的要求而设计的，以影像为导向的卡通着色器。

ユニティちゃんトゥーンシェーダーVer.2.0では、従来の機能に加えて大幅な機能強化を行いました。  
Ver.1.0でできる絵づくりをカバーしつつ、さらに高度なルックが実現できるようになっています。  
    unity酱toonshader ver.2.0在原有功能的基础上进行了大幅度的功能强化。
    涵盖了Ver.1.0能做的画制作的同时，还能实现更高级的外观。

● **[日本語マニュアル（v.2.0.8版）](https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project/blob/blob/release/legacy/2.0/Manual/UTS2_Manual_ja.md)が提供されています。合わせてご利用ください。**  


## 【Unity-Chan Toon Shader Ver.2.0.8】
Unity-Chan Toon Shader is a toon shader for video and images that is designed to meet your needs when creating cel-shaded 3DCG animations.  

    ##【Unity-Chan Toon Shader Ver.2.0.8】
    Unity-Chan卡通着色器是一个卡通着色器的视频和图像，是设计来满足您的需要时创建单元格阴影3DCG动画。

We have greatly enhanced the performance and features in Unity-Chan Toon Shader Ver. 2.0.  
It still has the same rendering capabilities as Ver. 1.0, but now you can give your creations an even more sophisticated look.  

    我们已经大大增强了Unity-Chan Toon Shader Ver. 2.0的性能和功能。
    它仍然具有与版本1.0相同的渲染功能，但现在您可以给您的创作一个更复杂的外观。
    
● **[English manual for v.2.0.8](https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project/blob/blob/release/legacy/2.0/Manual/UTS2_Manual_en.md) is available now.**  



----
## 【重要】旧バージョンから、直接v.2.0.8へバージョンアップをする場合の注意

* v.2.0.5以降は、そのままシェーダーのみ上書きアップデートをして大丈夫です。  
* v.2.0.4.3p1以前からアップデートをする場合、シェーダーを上書きアップデートした後で、各マテリアルをプロジェクトウィンドウ内から再度選択することで、マテリアルを更新してください。BaseMapが元通りに修復されます。  
* v.2.0.4.3p1以前からアップデートをする場合、HiColor_Powerのスライダの感度調整をした影響が出る場合があります。以下に従って調整をしてください。  
1. Is_SpecularToHighColor=OFF/Is_BlendAddToHiColor=0FFの場合には、HiColor_Powerの値を今までよりも低めに調整してください。  
2. Is_SpecularToHighColor=ONで利用している場合には、特に修正する必要はありません。  


## [Important] Note on upgrading to version 2.0.8 directly

* In v.2.0.5 or later, you can overwrite and update only the shader.  
* When updating from v.2.0.4.3p1 or earlier, update the materials by selecting each material again from within the project window after overwriting and updating the shader. BaseMap is restored as it was.  
* When updating from v.2.0.4.3p1 or earlier, the sensitivity of the slider of HiColor_Power may be affected. Please adjust according to the following.  
1. If Is_SpecularToHighColor = OFF / Is_BlendAddToHiColor = 0FF, adjust the HiColor_Power value lower than before.  
2. If you use Is_SpecularToHighColor = ON, there is no need to modify it.  

    ##[重要]直接升级到2.0.8版本
    *在v.2.0.5或更高版本中，你可以覆盖和更新仅着色器。
    *当从v.2.0.4.3p1或更早的版本更新时，在覆盖和更新着色器之后，通过在项目窗口中再次选择每个材质来更新材质。BaseMap恢复到原来的状态。
    *从v.2.0.4.3p1或更早版本更新时，HiColor_Power滑块的灵敏度可能会受到影响。请按照以下调整
    1. 如果Is_SpecularToHighColor = OFF / Is_BlendAddToHiColor = 0FF，调整HiColor_Power值低于之前。
    2. 如果你使用Is_SpecularToHighColor = ON，就不需要修改它了。

-----
## 【ターゲット環境】
Unity5.6.x もしくはそれ以降が必要です。  
Unity 2018.2.21f1からUnity 2019.2.0a9までの動作確認が終了しています。  
Unity 2017.4.15f1 LTSを含む、Unity 2017.4.x LTSでの動作確認済み。  
本パッケージは、Unity5.6.7f1で作成されています。  

### 【Target Environment】
Requires Unity 5.6.x or higher.  
The operation check from Unity 2018.2.21f1 to Unity 2019.2.0a9 has been completed.  
It has been tested on Unity 2017.4.x LTS, including Unity 2017.4.15f1 LTS.  
This pack was created in Unity 5.6.7f1.  

    ###【目标环境】
    5.6需要团结。x或更高。
    从Unity 2018.2.21f1到Unity 2019.2.0a9的操作检查已经完成。
    它已经在Unity 2017.4上测试过了。x LTS，包括Unity 2017.4.15f1 LTS。
    这个包是在Unity 5.6.7f1中创建的。

-----
## 【提供ライセンス】
「ユニティちゃんトゥーンシェーダーVer.2.0」は、UCL2.0（ユニティちゃんライセンス2.0）で提供されます。  
ユニティちゃんライセンスについては、以下を参照してください。  
https://unity-chan.com/contents/guideline/

### 【License】
Unity-Chan Toon Shader 2.0 is provided under the Unity-Chan License 2.0 terms.  
Please refer to the following link for information regarding the Unity-Chan License.  
https://unity-chan.com/contents/guideline_en/

    ###【许可证】
    unit - chan Toon Shader 2.0是根据unit - chan许可2.0条款提供的。
    有关Unity-Chan许可证的信息，请参考以下链接。
    https://unity-chan.com/contents/guideline_en/

-----
最新バージョン：2.0.8 Release  
最終リリース日：2021/09/08  
カテゴリー：3D  
形式：zip/unitypackage  

Latest Version: 2.0.8 Release  
Update: 2021/09/08  
Category: 3D  
File format: zip/unitypackage  

    最新版本:2.0.8 Release
    更新:2021/09/08
    类别:3 d
    文件格式:zip / unitypackage