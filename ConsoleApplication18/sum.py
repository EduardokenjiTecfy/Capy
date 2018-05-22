import sys
import cv2
import numpy as np
import os


img = cv2.imread(sys.argv[1])
gray = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY)
ret,thresh = cv2.threshold(gray,200,255,1)

# Remove some small noise if any.
dilate = cv2.dilate(thresh,None)
erode = cv2.erode(dilate,None)

# Find contours with cv2.RETR_CCOMP
image,contours,hierarchy = cv2.findContours(erode,cv2.RETR_LIST,cv2.CHAIN_APPROX_SIMPLE)
retorno =""
for cnt in contours:
    # Check if it is an external contour and its area is more than 100
    if  cv2.contourArea(cnt)>200:
        x,y,w,h = cv2.boundingRect(cnt)
        cv2.rectangle(img,(x,y),(x+w,y+h),(0,255,0),2)
        retorno +=str(x) +" "+  str(y)+" "+  str(w)+" "+  str(h)+"$"
        m = cv2.moments(cnt)
        cx,cy = m['m10']/m['m00'],m['m01']/m['m00']
 
print retorno
