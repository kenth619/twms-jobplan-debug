import { definePreset } from '@primeuix/themes'
import Lara from '@primeuix/themes/lara'

export const MyPreset = definePreset(Lara, {
    semantic: {
        primary: {
            50: '{green.50}',
            100: '{green.100}',
            200: '{green.200}',
            300: '{green.300}',
            400: '{green.400}',
            500: '{green.500}',
            600: '{green.600}',
            700: '{green.700}',
            800: '{green.800}',
            900: '{green.900}',
            950: '{green.950}',
        },

        components: {
            menu: {
                ColorScheme: {
                    light: {
                        separator: {
                            primary: {
                                color: '{neutral.800}',
                            },
                        },
                    },
                },
            },
        },

        // colorSceme: {
        //     light: {
        //         surface: {
        //             50: '{slate.50}',
        //             100: '{slate.100}',
        //             200: '{slate.200}',
        //             300: '{slate.300}',
        //             400: '{slate.400}',
        //             500: '{slate.500}',
        //             600: '{slate.600}',
        //             700: '{slate.700}',
        //             800: '{slate.800}',
        //             900: '{slate.900}',
        //             950: '{slate.950}',
        //         },

        //         dark: {
        //             surface: {
        //                 50: '{neutral.50}',
        //                 100: '{neutral.100}',
        //                 200: '{neutral.200}',
        //                 300: '{neutral.300}',
        //                 400: '{neutral.400}',
        //                 500: '{neutral.500}',
        //                 600: '{neutral.600}',
        //                 700: '{neutral.700}',
        //                 800: '{neutral.800}',
        //                 900: '{neutral.900}',
        //                 950: '{neutral.950}',
        //             },
        //         },
        //     },
        // },
    } },
)
