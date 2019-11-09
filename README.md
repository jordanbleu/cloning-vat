# Cloning Vat

## v0.0.1

### An Application for cloning one directory into one or more others.  Useful for backups.

## How to use

1. Create a file called ```CloneInstrutions.xml``` alongside the CloningVatApp.exe
2. This file should define the source / destination locations
3. Run the app 
4. The destination folders will be deleted and re-copied from the source file.  This is to ensure that they only contain exactly what the source file contains.  
5. You can also use the ```-skipui``` parameter to run the process without the verification screens

## Example XML

You can copy / paste the following XML file as an example, or you can just run the app without the ```CloneInstructions.xml``` and it will create an example for you

```XML
<Instructions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <CloneTasks>
    <From Path="{type a source directory}">
      <Destinations>
        <Into Path="{type a directory to clone into}" />
      </Destinations>
    </From>
  </CloneTasks>
</Instructions>
```

