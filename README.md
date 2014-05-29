codesec-hai
===========
Simple HAI Omni LTe Connector
Pulls sensor status and posts to Faye

Configuration
-------------
Requires following Configuration Settings in App.Config:
* fayeURL 
* HAC_IP - Home Automatio Controller IP
* HAC_Port - Home Automatio Controller IP
* HAC_Enc1 - Controller Encryption Key (Part 1)
* HAC_Enc2 - Controller Encryption Key (Part 2)

Sample App.Config
-----------------
'''
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key ="fayeURL" value = "http://10.10.10.1:9292/faye"/>
    <add key ="HAC_IP" value = "10.10.10.2"/>
    <add key ="HAC_Port" value = "4369"/>
    <add key ="HAC_Enc1" value = "0123456789ABCDEF"/>
    <add key ="HAC_Enc2" value = "FEDCBA9876543210"/>
  </appSettings>
</configuration>
'''

Dependencies
------------
Only dependency is:
* HAI.Controller.dll


