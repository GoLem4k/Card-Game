using System.IO;
using UnityEngine;

public class IntToTxtSaver : MonoBehaviour
{
    private string filePath;

    [ContextMenu("Open Save Folder")]
    public void OpenFolder()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
    
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "record.txt");
    }

    // Сохранение числа в txt
    public void SaveInt(int value)
    {
        File.WriteAllText(filePath, value.ToString());
        Debug.Log("Сохранено: " + value + " в " + filePath);
    }

    // Чтение числа из txt
    public int LoadInt()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Файл не найден. Возвращаю 0.");
            return 0;
        }

        string text = File.ReadAllText(filePath);

        if (int.TryParse(text, out int result))
        {
            return result;
        }

        Debug.Log("Не удалось преобразовать текст в число. Возвращаю 0.");
        return 0;
    }

    // Перезапись новым значением
    public void RewriteInt(int newValue)
    {
        SaveInt(newValue);
    }
}