When adding support for a new device:

- Remember that if the device uses a new library, then it should be compiled as its own DLL.

- For each new device, there should be at least two classes: a device class and a sensor class.

- The device class should adhere to these properties:
	- Must get a list of available devices or allow the user to specify an available device.
	- Must be able to create a sensor for the device.
	- Must support multiple sensors connected to it.
	- If device becomes dropped, it must propagate the drop event to all the connected sensors.

- The sensor class should adhere to these properties:
	- Must inherit from a sensor class in the SensorLib Base.
	- Should only be exposed using an interface.
	- If needed, the sensor class must tell the device class when it has become disposed.

