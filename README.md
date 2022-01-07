# What this does

payloadchecker is an application that checks the data written to your BOOT0 and compares it to known crc32's of spacecraft v1, spacecraft v2, spacecraft v2 with sd card improvements, original tx payload.

## Why this is important

Some people do not have access to original modchips which were upgradeable. This means the only options are clone chips.

HWFLY Lite chips are not reflashable.

HWFLY Core chips are not reflashable.

## Why does this matter?

Spacecraft v1 sets the following up:

```cpp
    /* Power on. */
    if (mariko)
    {
        uint8_t val = 0x3A;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_SD2, &val, 1);
        val = 0x71;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_SD2_CFG, &val, 1);
        val = 0xD0;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_LDO0_CFG, &val, 1);
    }
    else
    {
        uint8_t val = 0xD0;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_LDO0_CFG, &val, 1);
        val = 0x09;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_GPIO7, &val, 1);
    }
    
    /* Enable MIPI CAL, DSI, DISP1, HOST1X, UART_FST_MIPI_CAL, DSIA LP clocks. */
    car->rst_dev_h_clr = 0x1010000;
    car->clk_enb_h_set = 0x1010000;
    car->rst_dev_l_clr = 0x18000000;
    car->clk_enb_l_set = 0x18000000;
    car->clk_enb_x_set = 0x20000;
    car->clk_source_uart_fst_mipi_cal = 0xA;
    car->clk_enb_w_set = 0x80000;
    car->clk_source_dsia_lp = 0xA;
    /* DPD idle. */
    pmc->io_dpd_req = 0x40000000;
    pmc->io_dpd2_req = 0x40000000;
    /* Configure pins. */
    pinmux->nfc_en &= ~PINMUX_TRISTATE;
    pinmux->nfc_int &= ~PINMUX_TRISTATE;
    pinmux->lcd_bl_pwm &= ~PINMUX_TRISTATE;
    pinmux->lcd_bl_en &= ~PINMUX_TRISTATE;
    pinmux->lcd_rst &= ~PINMUX_TRISTATE;

    /* Configure Backlight +-5V GPIOs. */
    gpio_configure_mode(GPIO_LCD_BL_P5V, GPIO_MODE_GPIO);
    gpio_configure_mode(GPIO_LCD_BL_N5V, GPIO_MODE_GPIO);
    gpio_configure_direction(GPIO_LCD_BL_P5V, GPIO_DIRECTION_OUTPUT);
    gpio_configure_direction(GPIO_LCD_BL_N5V, GPIO_DIRECTION_OUTPUT);

    /* Enable Backlight +5V. */
    gpio_write(GPIO_LCD_BL_P5V, GPIO_LEVEL_HIGH); 

    udelay(10000);

    /* Enable Backlight -5V. */
    gpio_write(GPIO_LCD_BL_N5V, GPIO_LEVEL_HIGH); 

    udelay(10000);

    /* Configure Backlight PWM, EN and RST GPIOs. */
    gpio_configure_mode(GPIO_LCD_BL_PWM, GPIO_MODE_GPIO);
    gpio_configure_mode(GPIO_LCD_BL_EN, GPIO_MODE_GPIO);
    gpio_configure_mode(GPIO_LCD_BL_RST, GPIO_MODE_GPIO);    
    gpio_configure_direction(GPIO_LCD_BL_PWM, GPIO_DIRECTION_OUTPUT);
    gpio_configure_direction(GPIO_LCD_BL_EN, GPIO_DIRECTION_OUTPUT);
    gpio_configure_direction(GPIO_LCD_BL_RST, GPIO_DIRECTION_OUTPUT);

    /* Enable Backlight EN. */
    gpio_write(GPIO_LCD_BL_EN, GPIO_LEVEL_HIGH);
```

```cpp
    int aula = ((fuse_get_reserved_odm(4) & 0xF0000) >> 16) == 4;

    /* Power on. */
    if (mariko)
    {
        uint8_t val = 0x3A;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_SD2, &val, 1);
        val = 0x71;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_SD2_CFG, &val, 1);
        val = 0xD0;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_LDO0_CFG, &val, 1);
    }
    else
    {
        uint8_t val = 0xD0;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_LDO0_CFG, &val, 1);
        val = 0x09;
        i2c_send(I2C_5, MAX77620_PWR_I2C_ADDR, MAX77620_REG_GPIO7, &val, 1);
    }
    
    /* Enable MIPI CAL, DSI, DISP1, HOST1X, UART_FST_MIPI_CAL, DSIA LP clocks. */
    car->rst_dev_h_clr = 0x1010000;
    car->clk_enb_h_set = 0x1010000;
    car->rst_dev_l_clr = 0x18000000;
    car->clk_enb_l_set = 0x18000000;
    car->clk_enb_x_set = 0x20000;
    car->clk_source_uart_fst_mipi_cal = 0xA;
    car->clk_enb_w_set = 0x80000;
    car->clk_source_dsia_lp = 0xA;
    /* DPD idle. */
    pmc->io_dpd_req = 0x40000000;
    pmc->io_dpd2_req = 0x40000000;
    /* Configure pins. */
    pinmux->nfc_en &= ~PINMUX_TRISTATE;
    pinmux->nfc_int &= ~PINMUX_TRISTATE;
    pinmux->lcd_bl_pwm &= ~PINMUX_TRISTATE;
    pinmux->lcd_bl_en &= ~PINMUX_TRISTATE;
    pinmux->lcd_rst &= ~PINMUX_TRISTATE;

    if (!aula)
    {
	    /* Configure Backlight +-5V GPIOs. */
	    gpio_configure_mode(GPIO_LCD_BL_P5V, GPIO_MODE_GPIO);
	    gpio_configure_mode(GPIO_LCD_BL_N5V, GPIO_MODE_GPIO);
	    gpio_configure_direction(GPIO_LCD_BL_P5V, GPIO_DIRECTION_OUTPUT);
	    gpio_configure_direction(GPIO_LCD_BL_N5V, GPIO_DIRECTION_OUTPUT);

	    /* Enable Backlight +5V. */
	    gpio_write(GPIO_LCD_BL_P5V, GPIO_LEVEL_HIGH); 

	    udelay(10000);

	    /* Enable Backlight -5V. */
	    gpio_write(GPIO_LCD_BL_N5V, GPIO_LEVEL_HIGH); 

	    udelay(10000);

	    /* Configure Backlight PWM, EN and RST GPIOs. */
	    gpio_configure_mode(GPIO_LCD_BL_PWM, GPIO_MODE_GPIO);
	    gpio_configure_mode(GPIO_LCD_BL_EN, GPIO_MODE_GPIO);
    }

    gpio_configure_mode(GPIO_LCD_BL_RST, GPIO_MODE_GPIO);

    if (!aula)
    {
	    gpio_configure_direction(GPIO_LCD_BL_PWM, GPIO_DIRECTION_OUTPUT);
	    gpio_configure_direction(GPIO_LCD_BL_EN, GPIO_DIRECTION_OUTPUT);
    }

    gpio_configure_direction(GPIO_LCD_BL_RST, GPIO_DIRECTION_OUTPUT);

    if (!aula)
    {
	    /* Enable Backlight EN. */
	    gpio_write(GPIO_LCD_BL_EN, GPIO_LEVEL_HIGH);
    }
```
## Notice anything yet?
To point it out, Spacecraft v1 sets some pins to 5V that shouldn't be set to 5V on Aula (OLED) Switches. This is killing OLEDs. It's not immediate as there are tolerances and wherever this voltage goes on OLEDs doesn't always immediately kill it.


## So...what's this do?

This reads the payload data off of BOOT0 to determine the version of Spacecraft you are running. If you happened to install the chip into an OLED and it reads Spacecraft 0.1.0, take it out immediately or risk damaging your OLED and possibly kill it. I've already seen plenty of dead OLEDs due to the Chinese not using the latest Spacecraft release.
