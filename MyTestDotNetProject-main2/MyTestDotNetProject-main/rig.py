#!/usr/bin/env python3
import sys
import os

def main():
    if len(sys.argv) < 2:
        print("Usage: python rig.py <command> [options]")
        return
    
    command = sys.argv[1]
    
    if command == "build":
        print("Building application...")
        os.system("dotnet build --configuration Release")
        
    elif command == "deploy":
        print("Deploying infrastructure...")
        
    elif command == "install":
        print("Installing application...")
        
    else:
        print(f"Unknown command: {command}")

if __name__ == "__main__":
    main()
