import os
import time
import argparse
import subprocess

       
rootPath = "D:/EcologicalPlatform/GI/NeuralGI/UnityProject/unity-raytracing/Assets/RayTracing/Scenes/"
sceneDirs = []

for root, dirs, files in os.walk(rootPath):
    sceneDirs = dirs
    break

for scene in sceneDirs:
    path = rootPath + scene
 
    os.system("python convert_tungsten.py {} {}".format(path + '/scene.json', 'unity_scene'))