using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AssetPostprocessor 资源导入时的处理
/// </summary>
public class TextureSetting : AssetPostprocessor
{
	//private static FolderData _playerData;//静态 才能 存下来

	private void OnPreprocessTexture()
	{
		NamingConventions();
		SetTexturePara();
	}

	//判断命名是否规范
	private void NamingConventions()
	{
		PlayerNaming();
	}

	private void PlayerNaming()
	{
		//资源导入的时候会执行
		//if (assetPath.Contains(Paths.PICTURE_PLAYER_PICTURE_FOLDER))
		//{
		//	string name = Path.GetFileNameWithoutExtension(Path.GetFileName(assetPath));

		//	string pattern = "^[0-9]+_[0-9]+$";//^:以后面的模式作为起始   $:以前一项结尾

		//	Match result = Regex.Match(name, pattern);
		//	if (result.Value == "")
		//	{
		//		if (_playerData == null)
		//		{
		//			_playerData = new FolderData();
		//			_playerData.FolderPath = Paths.PICTURE_PLAYER_PICTURE_FOLDER;
		//			_playerData.NameTip = "0_0";
		//		}
				
		//		Debug.LogError("当前导入资源名称错误，名称为："+name);
		//		NameMgrWindowData.Add(_playerData,assetPath);
		//		NameMgrWindow.ShowWindow();
				
		//	}
		//}
	}
	
	private void SetTexturePara()
	{
		//assetImporter资源导入器
		TextureImporter importer = (TextureImporter) assetImporter;
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
	}
}
