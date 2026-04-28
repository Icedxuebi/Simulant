import os
import re
import shutil
from typing import List


RAWEXD_DIR = "rawexd"

def ensure_dir_for_file(path: str) -> None:
    """确保给定路径的父目录存在"""
    parent = os.path.dirname(path)
    if parent and not os.path.exists(parent):
        os.makedirs(parent, exist_ok=True)

def process_csv_files(rawexd_path: str, current_dir: str) -> None:
    """只更新当前目录中存在的 CSV 文件"""
    # 使用 rawexd_path 作为数据源的默认目录
    for root, _, files in os.walk(rawexd_path):
        for fn in files:
            if fn.lower().endswith(".csv"):
                new_csv_path = os.path.join(root, fn)
                dest_path = os.path.join(current_dir, fn)

                # 只在当前目录下存在的文件才进行覆盖
                if os.path.isfile(dest_path):
                    print(f"确认更新文件: {new_csv_path} 到 {dest_path}")
                    shutil.copy2(new_csv_path, dest_path)

def find_latest_version_dirs(base_dir: str) -> None:
    """查找最新的符合格式的版本文件夹"""
    # 版本目录的命名格式：YYYY.MM.DD.HHMM.SS
    version_dirs = []
    
    # 正则表达式用于验证文件夹名称是否符合版本号格式
    VERSION_DIR_RX = re.compile(r"^\d{4}\.\d{2}\.\d{2}\.\d{4}\.\d{4}$")
    
    # 获取指定目录下的所有文件夹
    for d in os.listdir(base_dir):
        full_path = os.path.join(base_dir, d)
        if os.path.isdir(full_path) and VERSION_DIR_RX.match(d):
            version_dirs.append(d)

    # 如果没有符合格式的文件夹
    if len(version_dirs) < 1:
        print("没有找到符合版本号格式的文件夹。")
        return []

    # 按照版本号降序排列文件夹
    version_dirs.sort(reverse=True)

    # 返回最新的版本文件夹
    return version_dirs[:1]  # 只取最新的版本文件夹

def get_rawexd_directory() -> str:
    """解析 rawexd 文件夹的位置，允许用户修改"""
    base_dir = os.getcwd()
    
    # 默认定位到 E:\FF14\SaintCoinach.Cmd\ 目录
    default_base_dir = r"E:\FF14\SaintCoinach.Cmd"
    if not os.path.isdir(default_base_dir):
        print(f"无法访问默认目录：{default_base_dir}")
        return None

    print(f"默认目录：{default_base_dir}")

    # 获取最新的版本文件夹
    version_dirs = find_latest_version_dirs(default_base_dir)
    if not version_dirs:
        print("未能找到符合版本格式的文件夹")
        return None

    latest_version_dir = os.path.join(default_base_dir, version_dirs[0])
    rawexd_path = os.path.join(latest_version_dir, RAWEXD_DIR)

    # 提供默认 rawexd 目录路径，并允许用户修改
    rawexd_path_input = input(f"默认的 rawexd 路径为: {rawexd_path}\n请输入新的 rawexd 目录路径 (直接按回车使用默认路径): ").strip()
    
    return rawexd_path_input if rawexd_path_input else rawexd_path

def ask_yes_no(prompt: str, default: str = "y") -> bool:
    """确认提示：是/否，允许修改输入的字符串"""
    while True:
        # 根据默认值填充提示，默认值是 'y'
        s = input(f"{prompt} (默认是'{default}'): ").strip().lower() or default
        if s in ("y", "yes", "1"):
            return True
        if s in ("n", "no", "0", ""):
            return False
        print("请输入 y 或 n（直接回车等同 n）")

def main() -> int:
    # 获取并允许用户修改 rawexd 目录
    rawexd_dir = get_rawexd_directory()
    if rawexd_dir is None:
        return 1

    current_dir = os.getcwd()  # 获取当前目录

    print(f"\n检测到以下目录：\n  rawexd 目录：{rawexd_dir}")
    if not ask_yes_no(f"是否继续更新已存在的 CSV 文件？", default="y"):
        print("操作已取消。")
        return 0

    print(f"\n开始更新CSV文件...\n")
    process_csv_files(rawexd_dir, current_dir)
    print(f"\nCSV文件更新完成，目录：{current_dir}")

    return 0

if __name__ == "__main__":
    try:
        exit_code = main()
    except Exception as e:
        print(f"发生错误：{e}")
    else:
        print(f"程序结束，ExitCode={exit_code}")