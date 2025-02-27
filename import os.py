import os

def count_lines_in_cs_files(directory):
    total_lines = 0
    file_count = 0
    
    # Проходим по всем файлам и папкам в директории
    for root, dirs, files in os.walk(directory):
        # Фильтруем только .cs файлы
        cs_files = [f for f in files if f.endswith('.cs')]
        
        for file in cs_files:
            file_path = os.path.join(root, file)
            try:
                with open(file_path, 'r', encoding='utf-8') as f:
                    # Подсчитываем непустые строки
                    lines = [line.strip() for line in f.readlines() if line.strip()]
                    file_lines = len(lines)
                    total_lines += file_lines
                    file_count += 1
                    print(f"{file_path}: {file_lines} строк")
            except Exception as e:
                print(f"Ошибка при чтении файла {file_path}: {str(e)}")
    
    return total_lines, file_count

def main():
    current_dir = os.getcwd()
    print(f"Анализ файлов в директории: {current_dir}")
    print("-" * 50)
    
    total_lines, file_count = count_lines_in_cs_files(current_dir)
    
    print("-" * 50)
    print(f"Всего файлов .cs: {file_count}")
    print(f"Общее количество строк: {total_lines}")
    
    if file_count > 0:
        avg_lines = total_lines / file_count
        print(f"Среднее количество строк на файл: {avg_lines:.2f}")

if __name__ == "__main__":
    main()