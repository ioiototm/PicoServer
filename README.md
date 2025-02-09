# PicoServer

PicoServer is a small application I put together in my free time to communicate with my Raspberry Pi Pico display. It collects system stats (like CPU, GPU, RAM, and network speeds) and sends them over serial or MQTT so my Pico display and Home Assistant instance can show them in real time.

I'm uploading it here in case someone else is working on something similar and finds it useful.

## Features
- Monitors CPU, GPU, RAM, and network stats.
- Sends data to a connected Raspberry Pi Pico display.
- Uses MQTT for remote monitoring.
- Runs quietly in the background and minimizes to the system tray.

## Configuration
There's a `config.json` file where you can set your MQTT server details, serial port, and other settings. This file is ignored in `.gitignore`, so make a copy of `config.example.json`, rename it to `config.json`, and fill in your details.

## Pico Display
This works alongside my Pico Display project, which receives and displays the data sent from this server. You can find that repo here: [**Pico Display Repo**](https://github.com/ioiototm/PicoDisplay).

## Notes
This was built for my own setup, so you might need to tweak things depending on your hardware and display setup. Also, big thanks to my ever-patient AI assistant for helping me clean up the code and make things work smoothly.

## License & Usage

This project is completely free to use, modify, and share. No restrictions, no copyright: just take it and do whatever you want with it! If you find it useful or make something cool with it, let me know, that'd be awesome! 